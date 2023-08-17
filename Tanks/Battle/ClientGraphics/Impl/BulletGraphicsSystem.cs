using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BulletGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Build(NodeAddedEvent e, BulletNode node, [JoinBy(typeof(TankGroupComponent))] WeaponNode weaponNode) {
            BulletEffectInstanceComponent bulletEffectInstanceComponent = new();
            bulletEffectInstanceComponent.Effect = Object.Instantiate(weaponNode.bulletEffect.BulletPrefab);
            BulletComponent bullet = node.bullet;
            bulletEffectInstanceComponent.Effect.transform.position = bullet.Position;
            bulletEffectInstanceComponent.Effect.transform.rotation = Quaternion.LookRotation(bullet.Direction);
            node.Entity.AddComponent(bulletEffectInstanceComponent);
        }

        [OnEventFire]
        public void Move(UpdateEvent e, BulletEffectNode node) {
            GameObject effect = node.bulletEffectInstance.Effect;
            BulletComponent bullet = node.bullet;
            effect.transform.position = bullet.Position;
            effect.transform.rotation = Quaternion.LookRotation(bullet.Direction);
        }

        [OnEventFire]
        public void Remove(NodeRemoveEvent e, BulletEffectNode bulletNode) =>
            Object.Destroy(bulletNode.bulletEffectInstance.Effect);

        [OnEventFire]
        public void Explosion(BulletHitEvent e, Node node, [JoinByTank] WeaponNode weaponNode) {
            BulletEffectComponent bulletEffect = weaponNode.bulletEffect;
            Object obj = Object.Instantiate(bulletEffect.ExplosionPrefab, e.Position, Quaternion.identity);
            Object.Destroy(obj, bulletEffect.ExplosionTime);
        }

        public class WeaponNode : Node {
            public BulletEffectComponent bulletEffect;
            public WeaponComponent weapon;
        }

        public class BulletNode : Node {
            public BulletComponent bullet;
            public TankGroupComponent tankGroup;
        }

        public class BulletEffectNode : Node {
            public BulletComponent bullet;

            public BulletConfigComponent bulletConfig;

            public BulletEffectInstanceComponent bulletEffectInstance;
        }
    }
}