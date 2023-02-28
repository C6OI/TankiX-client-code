using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TwinsBulletSystem : AbstractBulletSystem {
        [OnEventFire]
        public void Build(BulletBuildEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tankNode) {
            Entity entity = CreateEntity<TwinsBulletTemplate>("battle/weapon/twins/bullet");
            BulletComponent bulletComponent = new();
            WeaponBulletShotComponent weaponBulletShot = weaponNode.weaponBulletShot;
            bulletComponent.Speed = weaponBulletShot.BulletSpeed;
            bulletComponent.Radius = weaponBulletShot.BulletRadius;
            MuzzleVisualAccessor muzzleVisualAccessor = new(weaponNode.muzzlePoint);
            BulletConfigComponent bulletConfigComponent = entity.AddComponentAndGetInstance<BulletConfigComponent>();
            BulletTargetingComponent bulletTargetingComponent = new();
            bulletTargetingComponent.RadialRaysCount = bulletConfigComponent.RadialRaysCount;
            bulletTargetingComponent.Radius = bulletComponent.Radius;
            BulletTargetingComponent component = bulletTargetingComponent;
            Vector3 worldPosition = muzzleVisualAccessor.GetWorldPosition();
            Rigidbody rigidbody = tankNode.rigidbody.Rigidbody;
            bulletComponent.ShotId = weaponNode.shotId.ShotId;
            entity.AddComponent(component);
            TargetCollectorComponent component2 = new(new TargetCollector(tankNode.Entity), new TargetValidator(tankNode.Entity));
            entity.AddComponent(component2);
            InitBullet(bulletComponent, worldPosition, e.Direction, bulletComponent.Radius, rigidbody);
            entity.AddComponent(bulletComponent);
            entity.AddComponent(new TankGroupComponent(weaponNode.tankGroup.Key));
            entity.AddComponent<TwinsBulletComponent>();
            entity.AddComponent<ReadyBulletComponent>();
        }

        [OnEventComplete]
        public void HandleFrame(UpdateBulletEvent e, BulletNode bulletNode) {
            BulletComponent bullet = bulletNode.bullet;
            BulletConfigComponent bulletConfig = bulletNode.bulletConfig;
            DirectionData directionData = e.TargetingData.Directions[0];

            if (directionData.StaticHit != null) {
                bullet.Distance += (bullet.Position - directionData.StaticHit.Position).magnitude;
                SetPositionNearHitPoint(bullet, directionData.StaticHit.Position);
                SendBulletStaticHitEvent(bulletNode.Entity, bullet);
                DestroyBullet(bulletNode.Entity);
            } else if (!DestroyOnAnyTargetHit(bulletNode.Entity, bullet, bulletConfig, e.TargetingData)) {
                MoveBullet(bulletNode.Entity, bullet);

                if (bullet.Distance > bulletConfig.FullDistance) {
                    DestroyBullet(bulletNode.Entity);
                }
            }
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public RigidbodyComponent rigidbody;
        }

        public class BulletNode : Node {
            public BulletComponent bullet;

            public BulletConfigComponent bulletConfig;

            public ReadyBulletComponent readyBullet;
            public TankGroupComponent tankGroup;

            public TwinsBulletComponent twinsBullet;
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShotIdComponent shotId;
            public TankGroupComponent tankGroup;

            public TwinsComponent twins;

            public WeaponBulletShotComponent weaponBulletShot;
        }
    }
}