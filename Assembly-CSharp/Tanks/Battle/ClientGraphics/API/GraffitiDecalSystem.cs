using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class GraffitiDecalSystem : ECSSystem {
        const float SPRAY_DELAY = 5.1f;

        const float GRAFFITI_DELAY = 2f;

        const float ADDITIONAL_GUN_LENGTH = 10f;

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void InstantiateGraffitiSettings(NodeAddedEvent e, GraffitiBattleItemNode graffiti, [JoinAll] SingleNode<DecalManagerComponent> mapInstance) {
            GameObject gameObject = (GameObject)Object.Instantiate(graffiti.resourceData.Data);
            gameObject.AddComponent<GraffitiAntiSpamTimerComponent>();
            gameObject.AddComponent<EntityBehaviour>().BuildEntity(graffiti.Entity);
            graffiti.Entity.AddComponent(new GraffitiInstanceComponent(gameObject));
        }

        [OnEventComplete]
        public void InstantiateGraffitiSettings(NodeAddedEvent e, SingleNode<DecalManagerComponent> decalManager, [JoinAll] [Combine] GraffitiBattleItemNode graffiti) {
            GameObject gameObject = (GameObject)Object.Instantiate(graffiti.resourceData.Data);
            gameObject.AddComponent<GraffitiAntiSpamTimerComponent>();
            gameObject.AddComponent<EntityBehaviour>().BuildEntity(graffiti.Entity);
            graffiti.Entity.AddComponent(new GraffitiInstanceComponent(gameObject));
        }

        [OnEventFire]
        public void DestroyHolder(NodeRemoveEvent e, [Combine] SingleNode<GraffitiAntiSpamTimerComponent> graffitiHolder) {
            Object.Destroy(graffitiHolder.component.gameObject);
        }

        [OnEventFire]
        public void CheckSpraySelf(TimeUpdateEvent e, GraffitiSimpleNode graffiti, [JoinByUser] SingleNode<SelfBattleUserComponent> self,
            [JoinByUser] SingleNode<TankActiveStateComponent> tank, [JoinByBattle] SingleNode<RoundActiveStateComponent> round) {
            if (InputManager.GetActionKeyDown(BattleActions.GRAFFITI) && graffiti.graffitiAntiSpamTimer.SprayDelay < Time.realtimeSinceStartup) {
                ScheduleEvent<SprayEvent>(graffiti);
                graffiti.graffitiAntiSpamTimer.SprayDelay = Time.realtimeSinceStartup + 5.1f;
            }
        }

        [OnEventFire]
        public void OnRemoteGraffiti(NodeAddedEvent e, GraffitiNode node, [JoinByUser] SingleNode<UserUidComponent> UidNode, [JoinByUser] RemoteUserNode user) {
            string uid = UidNode.component.Uid;
            Vector3 sprayPosition = node.graffitiDecal.SprayPosition;
            Vector3 sprayDirection = node.graffitiDecal.SprayDirection;
            Vector3 sprayUpDirection = node.graffitiDecal.SprayUpDirection;
            GraffitiAntiSpamTimerComponent graffitiAntiSpamTimer = node.graffitiAntiSpamTimer;

            if (!graffitiAntiSpamTimer.GraffitiDelayDictionary.ContainsKey(uid)) {
                ScheduleEvent(new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection), node.Entity);
                GraffitiAntiSpamTimerComponent.GraffityInfo graffityInfo = new();
                graffityInfo.Time = Time.realtimeSinceStartup;
                GraffitiAntiSpamTimerComponent.GraffityInfo value = graffityInfo;
                graffitiAntiSpamTimer.GraffitiDelayDictionary.Add(uid, value);
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
                    graffityInfo2.CreateGraffitiEvent = new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection);
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
        public void DestroyGraffiti(NodeRemoveEvent e, GraffitiNode graffitiNode, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode,
            [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect) {
            if (graffitiNode.dynamicDecalProjector.Emmit) {
                bloomPostEffect.component.burningTargetBloom.targets.Remove(graffitiNode.graffitiInstance.EmitRenderer);
            }

            decalManagerNode.component.GraffitiDynamicDecalManager.RemoveDecal(graffitiNode.graffitiInstance.GraffitiDecalObject);
        }

        [OnEventFire]
        public void Spray(SprayEvent e, SingleNode<GraffitiInstanceComponent> graffitiInstanceNode, [JoinByUser] WeaponNode weaponNode) {
            MuzzleLogicAccessor muzzleLogicAccessor = new(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            Vector3 worldPosition = muzzleLogicAccessor.GetWorldPosition();
            Vector3 barrelOriginWorld = muzzleLogicAccessor.GetBarrelOriginWorld();
            Vector3 vector = worldPosition - barrelOriginWorld;
            float distance = (worldPosition - barrelOriginWorld).magnitude + 10f;
            int vISUAL_STATIC = LayerMasks.VISUAL_STATIC;
            RaycastHit hitInfo;

            if (PhysicsUtil.RaycastWithExclusion(barrelOriginWorld, vector, out hitInfo, distance, vISUAL_STATIC, null)) {
                ScheduleEvent(new CreateGraffitiEvent(barrelOriginWorld, vector, weaponNode.weaponInstance.WeaponInstance.transform.up), graffitiInstanceNode);
            }
        }

        void PlaySound(AudioSource soundPrefab, Vector3 position) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = soundPrefab.gameObject;
            getInstanceFromPoolEvent.AutoRecycleTime = soundPrefab.clip.length;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            Transform instance = getInstanceFromPoolEvent2.Instance;
            instance.position = position;
            AudioSource component = instance.GetComponent<AudioSource>();
            component.gameObject.SetActive(true);
            component.Play();
        }

        void DrawEffect(GraffitiVisualEffect prefab, float length, Transform parent, string spriteUid, ItemRarityType rarity) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = prefab.gameObject;
            getInstanceFromPoolEvent.AutoRecycleTime = length;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            Transform instance = getInstanceFromPoolEvent2.Instance;
            instance.parent = parent;
            instance.localPosition = Vector3.zero;
            instance.localRotation = Quaternion.identity;
            GraffitiVisualEffect component = instance.GetComponent<GraffitiVisualEffect>();
            component.Image.SpriteUid = spriteUid;
            component.Rarity = rarity;
            component.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, GraffitiSimpleNode graffitiNode, [JoinByUser] RemoteUserNode user, [JoinByUser] TankWithGraffitiNode tank,
            [JoinByUser] Optional<SingleNode<PremiumAccountBoostComponent>> premium, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode,
            [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect) {
            GameObject gameObject = DrawGraffiti(decalManagerNode.component, graffitiNode.dynamicDecalProjector, e.Position, e.Direction, e.Up);

            if ((bool)gameObject) {
                graffitiNode.graffitiInstance.GraffitiDecalObject = gameObject;
                PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
                AddEmitEffect(graffitiNode, bloomPostEffect.component);

                if (premium.IsPresent()) {
                    GraffitiVisualEffectComponent graffitiVisualEffect = tank.graffitiVisualEffect;
                    GraffitiVisualEffect visualEffectPrefab = graffitiVisualEffect.VisualEffectPrefab;
                    float timeToDestroy = graffitiVisualEffect.TimeToDestroy;
                    Transform transform = tank.tankCommonInstance.TankCommonInstance.transform;
                    string spriteUid = graffitiNode.imageItem.SpriteUid;
                    DrawEffect(visualEffectPrefab, timeToDestroy, transform, spriteUid, graffitiNode.itemRarity.RarityType);
                }
            }
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, FirstGraffitiNode graffitiNode, [JoinByUser] SingleNode<SelfBattleUserComponent> self, [JoinByUser] TankWithGraffitiNode tank,
            [JoinByUser] Optional<SingleNode<PremiumAccountBoostComponent>> premium, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode,
            [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect) {
            GameObject gameObject = DrawGraffiti(decalManagerNode.component, graffitiNode.dynamicDecalProjector, e.Position, e.Direction, e.Up);

            if ((bool)gameObject) {
                graffitiNode.graffitiInstance.GraffitiDecalObject = gameObject;
                graffitiNode.Entity.AddComponent(new GraffitiDecalComponent(e.Position, e.Direction, e.Up));
                PlaySound(graffitiNode.graffitiSound.Sound, e.Position);

                if (graffitiNode.dynamicDecalProjector.Emmit) {
                    Renderer component = graffitiNode.graffitiInstance.GraffitiDecalObject.GetComponent<Renderer>();
                    graffitiNode.graffitiInstance.EmitRenderer = component;
                    bloomPostEffect.component.burningTargetBloom.targets.Add(component);
                }

                if (premium.IsPresent()) {
                    GraffitiVisualEffectComponent graffitiVisualEffect = tank.graffitiVisualEffect;
                    GraffitiVisualEffect visualEffectPrefab = graffitiVisualEffect.VisualEffectPrefab;
                    float timeToDestroy = graffitiVisualEffect.TimeToDestroy;
                    Transform transform = tank.tankCommonInstance.TankCommonInstance.transform;
                    string spriteUid = graffitiNode.imageItem.SpriteUid;
                    DrawEffect(visualEffectPrefab, timeToDestroy, transform, spriteUid, graffitiNode.itemRarity.RarityType);
                }
            }
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, GraffitiNode graffitiNode, [JoinByUser] SingleNode<SelfBattleUserComponent> self, [JoinByUser] TankWithGraffitiNode tank,
            [JoinByUser] Optional<SingleNode<PremiumAccountBoostComponent>> premium, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode,
            [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect) {
            GameObject gameObject = DrawGraffiti(decalManagerNode.component, graffitiNode.dynamicDecalProjector, e.Position, e.Direction, e.Up);

            if ((bool)gameObject) {
                graffitiNode.Entity.RemoveComponent(typeof(GraffitiDecalComponent));
                graffitiNode.graffitiInstance.GraffitiDecalObject = gameObject;
                graffitiNode.Entity.AddComponent(new GraffitiDecalComponent(e.Position, e.Direction, e.Up));
                PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
                AddEmitEffect(graffitiNode, bloomPostEffect.component);

                if (premium.IsPresent()) {
                    GraffitiVisualEffectComponent graffitiVisualEffect = tank.graffitiVisualEffect;
                    GraffitiVisualEffect visualEffectPrefab = graffitiVisualEffect.VisualEffectPrefab;
                    float timeToDestroy = graffitiVisualEffect.TimeToDestroy;
                    Transform transform = tank.tankCommonInstance.TankCommonInstance.transform;
                    string spriteUid = graffitiNode.imageItem.SpriteUid;
                    DrawEffect(visualEffectPrefab, timeToDestroy, transform, spriteUid, graffitiNode.itemRarity.RarityType);
                }
            }
        }

        protected GameObject DrawGraffiti(DecalManagerComponent managerComponent, DynamicDecalProjectorComponent decalProjector, Vector3 position, Vector3 direction, Vector3 up) {
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
                return managerComponent.GraffitiDynamicDecalManager.AddGraffiti(mesh, decalProjector.Material, decalProjector.Color, decalProjector.LifeTime);
            }

            return null;
        }

        void AddEmitEffect(GraffitiSimpleNode graffiti, BurningTargetBloomComponent effect) {
            if (graffiti.dynamicDecalProjector.Emmit) {
                Renderer component = graffiti.graffitiInstance.GraffitiDecalObject.GetComponent<Renderer>();
                graffiti.graffitiInstance.EmitRenderer = component;
                effect.burningTargetBloom.targets.Add(component);
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, SelfBattleUserNode battleUser, [Context] [JoinByBattle] BattleNode battle, [Context] [JoinByMap] MapInstanceNode mapInstance,
            SingleNode<DecalManagerComponent> managerComponent) {
            managerComponent.component.GraffitiDynamicDecalManager = new GraffitiDynamicDecalManager(mapInstance.mapInstance.SceneRoot, battle.userLimit.UserLimit,
                battle.timeLimit.TimeLimitSec, managerComponent.component.DecalsQueue);
        }

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

        public class TankWithGraffitiNode : Node {
            public GraffitiVisualEffectComponent graffitiVisualEffect;
            public TankCommonInstanceComponent tankCommonInstance;
        }

        [Not(typeof(GraffitiDecalComponent))]
        public class FirstGraffitiNode : Node {
            public DynamicDecalProjectorComponent dynamicDecalProjector;
            public GraffitiInstanceComponent graffitiInstance;

            public GraffitiSoundComponent graffitiSound;

            public ImageItemComponent imageItem;

            public ItemRarityComponent itemRarity;
        }

        public class GraffitiSimpleNode : Node {
            public DynamicDecalProjectorComponent dynamicDecalProjector;

            public GraffitiAntiSpamTimerComponent graffitiAntiSpamTimer;
            public GraffitiInstanceComponent graffitiInstance;

            public GraffitiSoundComponent graffitiSound;

            public ImageItemComponent imageItem;

            public ItemRarityComponent itemRarity;
        }

        public class GraffitiNode : GraffitiSimpleNode {
            public GraffitiDecalComponent graffitiDecal;
        }

        [Not(typeof(SelfBattleUserComponent))]
        public class RemoteUserNode : Node {
            public BattleUserComponent battleUser;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;

            public MapGroupComponent mapGroup;

            public TimeLimitComponent timeLimit;

            public UserLimitComponent userLimit;
        }

        public class MapInstanceNode : Node {
            public MapGroupComponent mapGroup;
            public MapInstanceComponent mapInstance;
        }
    }
}