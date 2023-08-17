using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class GraffitiDecalSystem : ECSSystem {
        const float SPRAY_DELAY = 0.5f;

        const float GRAFFITI_DELAY = 2f;

        const float ADDITIONAL_GUN_LENGTH = 10f;

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void InstantiateGraffitiSettings(NodeAddedEvent e, GraffitiBattleItemNode graffiti,
            [JoinAll] SingleNode<DecalManagerComponent> mapInstance) {
            GameObject gameObject = (GameObject)Object.Instantiate(graffiti.resourceData.Data);
            gameObject.AddComponent<GraffitiAntiSpamTimerComponent>();
            gameObject.AddComponent<EntityBehaviour>().BuildEntity(graffiti.Entity);
            graffiti.Entity.AddComponent(new GraffitiInstanceComponent(gameObject));
        }

        [OnEventComplete]
        public void InstantiateGraffitiSettings(NodeAddedEvent e, SingleNode<DecalManagerComponent> mapInstance,
            [Combine] [JoinAll] GraffitiBattleItemNode graffiti) {
            GameObject gameObject = (GameObject)Object.Instantiate(graffiti.resourceData.Data);
            gameObject.AddComponent<GraffitiAntiSpamTimerComponent>();
            gameObject.AddComponent<EntityBehaviour>().BuildEntity(graffiti.Entity);
            graffiti.Entity.AddComponent(new GraffitiInstanceComponent(gameObject));
        }

        [OnEventFire]
        public void CheckSpraySelf(TimeUpdateEvent e, GraffitiSimpleNode graffiti,
            [JoinByUser] SingleNode<SelfBattleUserComponent> self, [JoinByUser] SingleNode<TankActiveStateComponent> tank,
            [JoinByBattle] SingleNode<RoundActiveStateComponent> round) {
            if (InputManager.GetActionKeyDown(BattleActions.GRAFFITI) &&
                graffiti.graffitiAntiSpamTimer.SprayDelay < Time.realtimeSinceStartup) {
                ScheduleEvent<SprayEvent>(graffiti);
                graffiti.graffitiAntiSpamTimer.SprayDelay = Time.realtimeSinceStartup + 0.5f;
            }
        }

        [OnEventFire]
        public void OnRemoteGraffiti(NodeAddedEvent e, GraffitiNode node, [JoinByUser] SingleNode<UserUidComponent> UidNode,
            [JoinByUser] RemoteUserNode user) {
            string uid = UidNode.component.Uid;
            Vector3 sprayPosition = node.graffitiDecal.SprayPosition;
            Vector3 sprayDirection = node.graffitiDecal.SprayDirection;
            Vector3 sprayUpDirection = node.graffitiDecal.SprayUpDirection;
            GraffitiAntiSpamTimerComponent graffitiAntiSpamTimer = node.graffitiAntiSpamTimer;

            if (!graffitiAntiSpamTimer.GraffitiDelayDictionary.ContainsKey(uid)) {
                ScheduleEvent(new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection), node.Entity);
                GraffitiAntiSpamTimerComponent.GraffityInfo graffityInfo = new();
                graffityInfo.Time = Time.realtimeSinceStartup;
                graffitiAntiSpamTimer.GraffitiDelayDictionary.Add(uid, graffityInfo);
                return;
            }

            float realtimeSinceStartup = Time.realtimeSinceStartup;
            GraffitiAntiSpamTimerComponent.GraffityInfo graffityInfo2 = graffitiAntiSpamTimer.GraffitiDelayDictionary[uid];
            float num = graffityInfo2.Time + 2f - realtimeSinceStartup;

            if (num > 0f) {
                if (graffityInfo2.Time > realtimeSinceStartup) {
                    graffityInfo2.CreateGraffitiEvent.Position = sprayPosition;
                    graffityInfo2.CreateGraffitiEvent.Direction = sprayDirection;
                    graffityInfo2.CreateGraffitiEvent.Up = sprayUpDirection;
                } else {
                    graffityInfo2.CreateGraffitiEvent =
                        new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection);

                    graffityInfo2.Time += 2f;
                    NewEvent(graffityInfo2.CreateGraffitiEvent).Attach(node.Entity).ScheduleDelayed(num);
                }
            } else {
                graffityInfo2.CreateGraffitiEvent = new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection);
                graffityInfo2.Time = Time.realtimeSinceStartup;
                NewEvent(graffityInfo2.CreateGraffitiEvent).Attach(node.Entity).Schedule();
            }
        }

        [OnEventFire]
        public void DestroyGraffiti(NodeRemoveEvent e, GraffitiNode graffitiNode,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) =>
            decalManagerNode.component.GraffitiDynamicDecalManager.RemoveDecal(graffitiNode.graffitiInstance
                .GraffitiDecalObject);

        [OnEventFire]
        public void Spray(SprayEvent e, SingleNode<GraffitiInstanceComponent> graffitiInstanceNode,
            [JoinByUser] WeaponNode weaponNode) {
            MuzzleLogicAccessor muzzleLogicAccessor = new(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            Vector3 worldPosition = muzzleLogicAccessor.GetWorldPosition();
            Vector3 barrelOriginWorld = muzzleLogicAccessor.GetBarrelOriginWorld();
            Vector3 vector = worldPosition - barrelOriginWorld;
            float distance = (worldPosition - barrelOriginWorld).magnitude + 10f;
            int vISUAL_STATIC = LayerMasks.VISUAL_STATIC;
            RaycastHit hitInfo;

            if (PhysicsUtil.RaycastWithExclusion(barrelOriginWorld, vector, out hitInfo, distance, vISUAL_STATIC, null)) {
                Vector3 pulledHitPoint = PhysicsUtil.GetPulledHitPoint(hitInfo);

                ScheduleEvent(new CreateGraffitiEvent(barrelOriginWorld,
                        vector,
                        weaponNode.weaponInstance.WeaponInstance.transform.up),
                    graffitiInstanceNode);
            }
        }

        void PlaySound(AudioSource soundPrefab, Vector3 position) {
            AudioSource audioSource = Object.Instantiate(soundPrefab, position, Quaternion.identity);
            audioSource.Play();
            Object.Destroy(audioSource.gameObject, audioSource.clip.length);
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, GraffitiSimpleNode graffitiNode, [JoinByUser] RemoteUserNode user,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) {
            GameObject gameObject = DrawGraffiti(decalManagerNode.component,
                graffitiNode.dynamicDecalProjector,
                e.Position,
                e.Direction,
                e.Up);

            if ((bool)gameObject) {
                graffitiNode.graffitiInstance.GraffitiDecalObject = gameObject;
                PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
            }
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, FirstGraffitiNode graffitiNode,
            [JoinByUser] SingleNode<SelfBattleUserComponent> self,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) {
            GameObject gameObject = DrawGraffiti(decalManagerNode.component,
                graffitiNode.dynamicDecalProjector,
                e.Position,
                e.Direction,
                e.Up);

            if ((bool)gameObject) {
                graffitiNode.graffitiInstance.GraffitiDecalObject = gameObject;
                graffitiNode.Entity.AddComponent(new GraffitiDecalComponent(e.Position, e.Direction, e.Up));
                PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
            }
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, GraffitiNode graffitiNode,
            [JoinByUser] SingleNode<SelfBattleUserComponent> self,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) {
            GameObject gameObject = DrawGraffiti(decalManagerNode.component,
                graffitiNode.dynamicDecalProjector,
                e.Position,
                e.Direction,
                e.Up);

            if ((bool)gameObject) {
                graffitiNode.Entity.RemoveComponent(typeof(GraffitiDecalComponent));
                graffitiNode.graffitiInstance.GraffitiDecalObject = gameObject;
                graffitiNode.Entity.AddComponent(new GraffitiDecalComponent(e.Position, e.Direction, e.Up));
                PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
            }
        }

        protected GameObject DrawGraffiti(DecalManagerComponent managerComponent,
            DynamicDecalProjectorComponent decalProjector, Vector3 position, Vector3 direction, Vector3 up) {
            DecalProjection decalProjection = new();
            decalProjection.AtlasHTilesCount = decalProjector.AtlasHTilesCount;
            decalProjection.AtlasVTilesCount = decalProjector.AtlasVTilesCount;
            decalProjection.SurfaceAtlasPositions = decalProjector.SurfaceAtlasPositions;
            decalProjection.HalfSize = decalProjector.HalfSize;
            decalProjection.Up = up;
            decalProjection.Distantion = decalProjector.Distance;
            decalProjection.Ray = new Ray(position, direction);
            DecalProjection decalProjection2 = decalProjection;
            Mesh mesh = null;

            if (managerComponent.DecalMeshBuilder.Build(decalProjection2, ref mesh)) {
                return managerComponent.GraffitiDynamicDecalManager.AddGraffiti(mesh,
                    decalProjector.Material,
                    decalProjector.Color,
                    decalProjector.LifeTime);
            }

            return null;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, SingleNode<DecalManagerComponent> managerComponent,
            [JoinAll] SingleNode<MapInstanceComponent> mapInstance, [JoinAll] SingleNode<TimeLimitComponent> roundTimeLimit,
            [JoinAll] SingleNode<UserLimitComponent> userLimit) => managerComponent.component.GraffitiDynamicDecalManager =
                                                                       new GraffitiDynamicDecalManager(
                                                                           mapInstance.component.SceneRoot,
                                                                           userLimit.component.UserLimit,
                                                                           roundTimeLimit.component.TimeLimitSec,
                                                                           managerComponent.component.DecalsQueue);

        public class GraffitiBattleItemNode : Node {
            public GraffitiBattleItemComponent graffitiBattleItem;

            public ResourceDataComponent resourceData;
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public TankGroupComponent tankGroup;

            public WeaponComponent weapon;
            public WeaponInstanceComponent weaponInstance;
        }

        [Not(typeof(GraffitiDecalComponent))]
        public class FirstGraffitiNode : Node {
            public DynamicDecalProjectorComponent dynamicDecalProjector;
            public GraffitiInstanceComponent graffitiInstance;

            public GraffitiSoundComponent graffitiSound;
        }

        public class GraffitiSimpleNode : Node {
            public DynamicDecalProjectorComponent dynamicDecalProjector;

            public GraffitiAntiSpamTimerComponent graffitiAntiSpamTimer;
            public GraffitiInstanceComponent graffitiInstance;

            public GraffitiSoundComponent graffitiSound;
        }

        public class GraffitiNode : GraffitiSimpleNode {
            public GraffitiDecalComponent graffitiDecal;
        }

        [Not(typeof(SelfBattleUserComponent))]
        public class RemoteUserNode : Node {
            public BattleUserComponent battleUser;
        }
    }
}