using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TargetFocusEffectSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void AddTargetFocusComponent(NodeAddedEvent e, SelfTankNode tank, SingleNode<TargetFocusSettingsComponent> settings) {
            if (settings.component.Enabled) {
                tank.Entity.AddComponent<TargetFocusEffectComponent>();
                tank.Entity.AddComponent<TargetFocusVerticalTargetingComponent>();
                tank.Entity.AddComponent<TargetFocusVerticalSectorTargetingComponent>();
                tank.Entity.AddComponent<TargetFocusConicTargetingComponent>();
                tank.Entity.AddComponent<TargetFocusPelletConeComponent>();
            }
        }

        [OnEventFire]
        public void ApplyTargetFocusEffect(NodeAddedEvent e, SelfTankNode tank, [JoinByTank] [Context] ConicTargetingNode weapon, [JoinByTank] [Context] TargetFocusEffectNode effect) {
            if (!weapon.Entity.HasComponent<WeaponPreparedByTargetFocusEffectComponent>()) {
                float halfConeAngle = weapon.conicTargeting.HalfConeAngle;
                float num = (float)weapon.conicTargeting.HalfConeNumRays / halfConeAngle;
                weapon.conicTargeting.HalfConeAngle += effect.targetFocusConicTargeting.AdditionalHalfConeAngle;
                weapon.conicTargeting.HalfConeNumRays = Mathf.RoundToInt(num * weapon.conicTargeting.HalfConeAngle);
                weapon.Entity.AddComponent<WeaponPreparedByTargetFocusEffectComponent>();
            }
        }

        [OnEventFire]
        public void ApplyTargetFocusAngles(NodeAddedEvent e, TargetFocusEffectNode effect, [JoinByTank] [Context] VerticalSectorTargetingNode weapon,
            [JoinByTank] [Context] SelfTankNode tank) {
            if (!weapon.Entity.HasComponent<WeaponPreparedByTargetFocusEffectComponent>()) {
                weapon.verticalSectorsTargeting.VAngleDown += effect.targetFocusVerticalSectorTargeting.AdditionalAngleDown;
                weapon.verticalSectorsTargeting.VAngleUp += effect.targetFocusVerticalSectorTargeting.AdditionalAngleUp;
                weapon.verticalSectorsTargeting.HAngle += effect.targetFocusVerticalSectorTargeting.AdditionalAngleHorizontal;
                weapon.Entity.AddComponent<WeaponPreparedByTargetFocusEffectComponent>();
            }
        }

        [OnEventFire]
        public void ApplyTargetFocusAngles(NodeAddedEvent e, TargetFocusEffectNode effect, [JoinByTank] [Context] VerticalTargetingNode weapon, [JoinByTank] [Context] SelfTankNode tank) {
            if (!weapon.Entity.HasComponent<WeaponPreparedByTargetFocusEffectComponent>()) {
                float angleDown = weapon.verticalTargeting.AngleDown;
                float angleUp = weapon.verticalTargeting.AngleUp;
                float num = (float)weapon.verticalTargeting.NumRaysUp / angleUp;
                float num2 = (float)weapon.verticalTargeting.NumRaysDown / angleDown;
                weapon.verticalTargeting.AngleDown += effect.targetFocusVerticalTargeting.AdditionalAngleDown;
                weapon.verticalTargeting.AngleUp += effect.targetFocusVerticalTargeting.AdditionalAngleUp;
                weapon.verticalTargeting.NumRaysUp = Mathf.RoundToInt(num * weapon.verticalTargeting.AngleUp);
                weapon.verticalTargeting.NumRaysDown = Mathf.RoundToInt(num2 * weapon.verticalTargeting.AngleDown);
                weapon.Entity.AddComponent<WeaponPreparedByTargetFocusEffectComponent>();
            }
        }

        [OnEventFire]
        public void SetTeam(NodeAddedEvent e, ReticleResourceNode reticleNode, SelfWithTeam self, [JoinByUser] SingleNode<IsisComponent> isis) {
            reticleNode.reticle.CanHeal = true;
            reticleNode.reticle.TeamKey = self.teamGroup.Key;
        }

        [OnEventFire]
        public void OnEnableEffect(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, WeaponWithReticleNode reticleTemplate, [JoinByTank] [Context] TargetFocusEffectNode effect,
            [JoinByTank] [Context] SelfTankNode selfTank, [JoinAll] Optional<SingleNode<ReticleComponent>> reticle) {
            if (!reticle.IsPresent()) {
                CreateEntity(reticleTemplate.reticleTemplate.Template.TemplateId, reticleTemplate.reticleTemplate.ConfigPath);
            } else {
                reticle.Get().component.Reset();
            }
        }

        [OnEventFire]
        public void CreateReticle(NodeAddedEvent e, ReticleResourceNode reticleNode, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode, [JoinAll] SelfTankNode selfTank,
            [JoinByUser] WeaponPartNode weapon) {
            reticleNode.reticle.Hammer = weapon.Entity.HasComponent<HammerComponent>();
            reticleNode.reticle.CanvasSize = canvasNode.component.screensLayer.rect.size;
            reticleNode.reticle.Create(reticleNode.resourceData.Data, canvasNode.component.transform);
            selfTank.tankGroup.Attach(reticleNode.Entity);
        }

        [OnEventFire]
        public void OnReticle(TankMovementInitEvent e, SingleNode<SelfTankComponent> tank, [JoinByTank] SingleNode<WeaponInstanceComponent> weaponInstance,
            [JoinByTank] SingleNode<SelfTankComponent> self, [JoinAll] SingleNode<ReticleComponent> reticleNode) {
            reticleNode.component.Reset();
        }

        [OnEventFire]
        public void OffReticle(NodeRemoveEvent e, SingleNode<TankActiveStateComponent> activeTank, [JoinByTank] SingleNode<SelfTankComponent> self,
            [JoinAll] SingleNode<ReticleComponent> reticleNode) {
            reticleNode.component.Deactivate();
        }

        [OnEventFire]
        public void RemoveReticle(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<ReticleComponent> reticleNode) {
            reticleNode.component.Destroy();
            DeleteEntity(reticleNode.Entity);
        }

        [OnEventFire]
        public void UpdateReticle(UpdateEvent e, SelfActiveTankNode activeTank, [JoinByTank] WeaponNode weapon, [JoinByTank] Optional<WeaponWithLaser> weaponWithLaserNode,
            [JoinAll] SingleNode<ScreensLayerComponent> canvasNode, [JoinAll] Optional<SingleNode<ReticleComponent>> reticleNode,
            [JoinAll] SingleNode<LaserSightSettingsComponent> settings) {
            ReticleComponent reticleComponent = null;
            WeaponWithLaser weaponWithLaser = null;

            if (reticleNode.IsPresent()) {
                reticleComponent = reticleNode.Get().component;
            }

            if (weaponWithLaserNode.IsPresent() && settings.component.Enabled) {
                weaponWithLaser = weaponWithLaserNode.Get();
            }

            if (reticleComponent != null && weapon.Entity.HasComponent<ShaftComponent>() && (weapon.Entity.HasComponent<ShaftAimingWorkingStateComponent>() ||
                                                                                             weapon.Entity.HasComponent<ShaftAimingWorkFinishStateComponent>() ||
                                                                                             weapon.Entity.HasComponent<ShaftAimingWorkActivationStateComponent>())) {
                reticleComponent.SetFree();
            } else {
                if (reticleComponent == null && weaponWithLaser == null) {
                    return;
                }

                if (!weapon.Entity.HasComponent<WeaponUnblockedComponent>() && reticleComponent != null && weaponWithLaser == null) {
                    reticleComponent.SetFree();
                    return;
                }

                TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
                ScheduleEvent(BattleCache.targetingEvent.GetInstance().Init(targetingData), weapon);

                if (reticleComponent != null) {
                    if (targetingData.HasTargetHit() && targetingData.BestDirection != null && targetingData.BestDirection.Targets.Count > 0) {
                        reticleComponent.SetTargets(targetingData.BestDirection.Targets, canvasNode.component.screensLayer.rect.size);
                    } else {
                        reticleComponent.SetFree();
                    }
                }

                if (weaponWithLaser != null) {
                    UpdateLaser(targetingData, weaponWithLaser);
                }
            }
        }

        [OnEventFire]
        public void InitLaser(NodeAddedEvent e, SelfActiveTankNode activeTank, [JoinByTank] WeaponWithLaser weapon, [JoinAll] SingleNode<LaserSightSettingsComponent> settings) {
            if (settings.component.Enabled) {
                GameObject asset = weapon.shaftAimingLaser.Asset;

                for (int i = 0; i < weapon.muzzlePoint.Points.Length; i++) {
                    GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
                    getInstanceFromPoolEvent.Prefab = asset;
                    GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
                    ScheduleEvent(getInstanceFromPoolEvent2, activeTank);
                    Transform instance = getInstanceFromPoolEvent2.Instance;
                    GameObject gameObject = instance.gameObject;
                    ShaftAimingLaserBehaviour component = gameObject.GetComponent<ShaftAimingLaserBehaviour>();
                    weapon.shaftAimingLaser.EffectInstance = component;
                    CustomRenderQueue.SetQueue(gameObject, 3150);
                    Transform transform = weapon.weaponInstance.WeaponInstance.transform;
                    instance.position = transform.position;
                    instance.rotation = transform.rotation;
                    gameObject.SetActive(true);
                    component.Init();
                    component.SetColor(Color.red);
                    component.Show();
                }

                weapon.Entity.AddComponent<ShaftAimingLaserReadyComponent>();
            }
        }

        [OnEventFire]
        public void DeinitLaser(NodeRemoveEvent e, SelfActiveTankNode activeTank, [JoinByTank] WeaponWithLaser weapon) {
            foreach (ShaftAimingLaserBehaviour effectInstance in weapon.shaftAimingLaser.EffectInstances) {
                if (effectInstance != null) {
                    effectInstance.Kill();
                }
            }

            weapon.Entity.RemoveComponentIfPresent<ShaftAimingLaserReadyComponent>();
        }

        [OnEventFire]
        public void DeinitLaser(NodeRemoveEvent e, WeaponWithLaser weapon) {
            foreach (ShaftAimingLaserBehaviour effectInstance in weapon.shaftAimingLaser.EffectInstances) {
                if (effectInstance != null) {
                    effectInstance.Kill();
                }
            }

            weapon.Entity.RemoveComponentIfPresent<ShaftAimingLaserReadyComponent>();
        }

        void UpdateLaser(TargetingData targeting, WeaponWithLaser weapon) {
            float maxLength = weapon.shaftAimingLaser.MaxLength;
            float minLength = weapon.shaftAimingLaser.MinLength;

            for (int i = 0; i < weapon.muzzlePoint.Points.Length; i++) {
                Vector3 forward = weapon.weaponInstance.WeaponInstance.transform.forward;
                Transform transform = weapon.muzzlePoint.Points[i];
                Vector3 vector = transform.position - forward * transform.localPosition.magnitude;
                ShaftAimingLaserBehaviour shaftAimingLaserBehaviour = weapon.shaftAimingLaser.EffectInstances[i];
                bool showLaser;
                bool showSpot;

                if (targeting.HasTargetHit() && targeting.BestDirection != null && targeting.BestDirection.Targets.Count > 0) {
                    Vector3 hitPoint = targeting.BestDirection.Targets[0].HitPoint;
                    float num = Vector3.Distance(vector, hitPoint);
                    showLaser = num >= minLength + transform.localPosition.magnitude;
                    showSpot = num > transform.localPosition.magnitude;
                    shaftAimingLaserBehaviour.UpdateTargetPosition(vector, hitPoint, showLaser, showSpot);
                    continue;
                }

                MeshCollider playerWeaponCollider = weapon.weaponVisualRoot.VisualTriggerMarker.VisualTriggerMeshCollider;
                List<RaycastHit> list = Physics.RaycastAll(vector, forward, maxLength, LayerMasks.VISUAL_TARGETING).ToList();
                list.RemoveAll((RaycastHit x) => x.collider == playerWeaponCollider);
                Vector3 vector2;

                if (list.Count > 0) {
                    list.Sort((RaycastHit a, RaycastHit b) => !(a.distance < b.distance) ? 1 : -1);
                    vector2 = list[0].point;
                    showLaser = Vector3.Distance(vector, vector2) >= minLength + transform.localPosition.magnitude;
                    showSpot = Vector3.Distance(vector, vector2) > transform.localPosition.magnitude;
                } else {
                    showLaser = true;
                    showSpot = false;
                    vector2 = vector + forward * maxLength;
                }

                shaftAimingLaserBehaviour.UpdateTargetPosition(vector, vector2, showLaser, showSpot);
                weapon.shaftAimingLaser.CurrentLaserDirection = forward;
            }
        }

        public class TargetFocusEffectNode : Node {
            public TankGroupComponent tankGroup;

            public TargetFocusConicTargetingComponent targetFocusConicTargeting;
            public TargetFocusEffectComponent targetFocusEffect;

            public TargetFocusPelletConeComponent targetFocusPelletCone;

            public TargetFocusVerticalSectorTargetingComponent targetFocusVerticalSectorTargeting;

            public TargetFocusVerticalTargetingComponent targetFocusVerticalTargeting;
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponInstanceComponent weaponInstance;
        }

        public class WeaponPartNode : Node {
            public TankPartComponent tankPart;
            public WeaponComponent weapon;
        }

        [Not(typeof(ShaftComponent))]
        public class WeaponWithLaser : WeaponNode {
            public ShaftAimingLaserComponent shaftAimingLaser;
            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class ReticleResourceNode : Node {
            public AssetReferenceComponent assetReference;

            public ResourceDataComponent resourceData;
            public ReticleComponent reticle;
        }

        public class WeaponWithReticleNode : Node {
            public ReticleTemplateComponent reticleTemplate;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;
        }

        public class SelfActiveTankNode : Node {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }

        public class SelfWithTeam : Node {
            public SelfTankComponent selfTank;

            public TeamGroupComponent teamGroup;
        }

        public class VerticalTargetingNode : Node {
            public TankGroupComponent tankGroup;
            public VerticalTargetingComponent verticalTargeting;
        }

        public class VerticalSectorTargetingNode : Node {
            public TankGroupComponent tankGroup;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
        }

        public class ConicTargetingNode : Node {
            public ConicTargetingComponent conicTargeting;

            public TankGroupComponent tankGroup;
        }
    }
}