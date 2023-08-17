using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RicochetBulletBounceGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void SpawnExplosionOnBounce(UpdateBulletEvent e, BulletNode bulletNode, [JoinByTank] WeaponNode weaponNode) {
            BulletEffectComponent bulletEffect = weaponNode.bulletEffect;
            DirectionData directionData = e.TargetingData.Directions.First();

            if (directionData.StaticHit != null) {
                Object obj = Object.Instantiate(bulletEffect.ExplosionPrefab,
                    directionData.StaticHit.Position + directionData.StaticHit.Normal * bulletEffect.ExplosionOffset,
                    Quaternion.LookRotation(directionData.StaticHit.Normal));

                Object.Destroy(obj, bulletEffect.ExplosionTime);
            }
        }

        public class WeaponNode : Node {
            public BulletEffectComponent bulletEffect;

            public RicochetComponent ricochet;
            public TankGroupComponent tankGroup;
        }

        public class BulletNode : Node {
            public RicochetBulletComponent ricochetBullet;
            public TankGroupComponent tankGroup;
        }
    }
}