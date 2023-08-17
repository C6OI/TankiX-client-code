using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BulletSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void PrepareTargetsAtFirstFrame(NodeAddedEvent e, BulletNode bulletNode, [JoinByTank] WeaponNode weaponNode) {
            BulletComponent bullet = bulletNode.bullet;
            BulletConfigComponent bulletConfig = bulletNode.bulletConfig;

            Vector3 barrelOriginWorld = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance)
                .GetBarrelOriginWorld();

            float fullDistance = bullet.Speed * Time.deltaTime + (bullet.Position - barrelOriginWorld).magnitude;
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            targetingData.Origin = barrelOriginWorld;
            targetingData.Dir = bullet.Direction;
            targetingData.FullDistance = fullDistance;
            ScheduleEvent(BattleCache.targetingEvent.GetInstance().Init(targetingData), bulletNode);
            ScheduleEvent(BattleCache.updateBulletEvent.GetInstance().Init(targetingData), bulletNode);
        }

        [OnEventFire]
        public void PrepareTargets(UpdateEvent e, BulletNode bulletNode) {
            BulletComponent bullet = bulletNode.bullet;
            BulletConfigComponent bulletConfig = bulletNode.bulletConfig;
            float num = UnityTime.time - bullet.LastUpdateTime;
            float val = Math.Max(0f, bulletConfig.FullDistance - bullet.Distance);
            float fullDistance = Math.Min(val, bullet.Speed * num);
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            targetingData.Origin = bullet.Position - bullet.Direction * 0.1f;
            targetingData.Dir = bullet.Direction;
            targetingData.FullDistance = fullDistance;
            ScheduleEvent(BattleCache.targetingEvent.GetInstance().Init(targetingData), bulletNode);
            ScheduleEvent(BattleCache.updateBulletEvent.GetInstance().Init(targetingData), bulletNode);
        }

        [OnEventComplete]
        public void SendHitEvent(TargetingEvent e, SingleNode<BulletComponent> bulletNode,
            [JoinByTank] UnblockedWeaponNode weaponNode, [JoinByTank] SingleNode<SelfTankComponent> tankNode) {
            if (e.TargetingData.HasBaseStaticHit()) {
                return;
            }

            foreach (DirectionData direction in e.TargetingData.Directions) {
                if (direction.HasTargetHit()) {
                    TargetData targetData = direction.Targets.First();

                    if (targetData.Entity.HasComponent<TankActiveStateComponent>()) {
                        PrepareTargetData(targetData, bulletNode.component);
                        SelfHitEvent selfHitEvent = new();
                        selfHitEvent.Targets = new List<HitTarget> { HitTargetAdapter.Adapt(targetData) };
                        selfHitEvent.ShotId = bulletNode.component.ShotId;
                        ScheduleEvent(selfHitEvent, weaponNode.Entity);
                        break;
                    }
                }
            }
        }

        [OnEventFire]
        public void DestroyBulletsOnRemoveWeapon(NodeRemoveEvent e, WeaponNode weapon,
            [Combine] [JoinByTank] BulletNode bullet) {
            bullet.Entity.RemoveComponent<BulletComponent>();
            DeleteEntity(bullet.Entity);
        }

        protected void PrepareTargetData(TargetData targetData, BulletComponent bulletComponent) {
            targetData.HitDistance += bulletComponent.Distance;
            targetData.HitDirection = bulletComponent.Direction;
        }

        public class BulletNode : Node {
            public BulletComponent bullet;

            public BulletConfigComponent bulletConfig;

            public BulletTargetCollectorComponent bulletTargetCollector;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponInstanceComponent weaponInstance;
        }

        public class UnblockedWeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public TankGroupComponent tankGroup;
        }
    }
}