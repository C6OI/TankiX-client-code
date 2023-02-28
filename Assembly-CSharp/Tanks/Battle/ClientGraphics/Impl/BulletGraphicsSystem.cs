using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BulletGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Build(NodeAddedEvent e, BulletNode node, [JoinBy(typeof(TankGroupComponent))] WeaponNode weaponNode) {
            BulletComponent bullet = node.bullet;
            Quaternion rotation = Quaternion.LookRotation(bullet.Direction);
            BulletEffectInstanceComponent bulletEffectInstanceComponent = new();
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = weaponNode.bulletEffect.BulletPrefab;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, node);
            GameObject gameObject = getInstanceFromPoolEvent2.Instance.gameObject;
            gameObject.transform.position = bullet.Position;
            gameObject.transform.rotation = rotation;
            gameObject.SetActive(true);
            bulletEffectInstanceComponent.Effect = gameObject;
            CustomRenderQueue.SetQueue(gameObject, 3150);
            node.Entity.AddComponent(bulletEffectInstanceComponent);
        }

        [OnEventFire]
        public void Move(UpdateEvent e, BulletEffectNode node) {
            GameObject effect = node.bulletEffectInstance.Effect;

            if ((bool)effect) {
                BulletComponent bullet = node.bullet;
                effect.transform.position = bullet.Position;
                effect.transform.rotation = Quaternion.LookRotation(bullet.Direction);
            }
        }

        [OnEventFire]
        public void Remove(NodeRemoveEvent e, BulletEffectNode bulletNode) {
            bulletNode.bulletEffectInstance.Effect.RecycleObject();
        }

        [OnEventFire]
        public void Explosion(BulletStaticHitEvent e, Node node, [JoinByTank] WeaponNode weaponNode) {
            InstantiateExplosion(e, weaponNode);
        }

        [OnEventFire]
        public void Explosion(BulletTargetHitEvent e, Node node, [JoinByTank] WeaponNode weaponNode) {
            InstantiateExplosion(e, weaponNode);
        }

        void InstantiateExplosion(BulletHitEvent e, WeaponNode weaponNode) {
            BulletEffectComponent bulletEffect = weaponNode.bulletEffect;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = bulletEffect.ExplosionPrefab;
            getInstanceFromPoolEvent.AutoRecycleTime = bulletEffect.ExplosionTime;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, weaponNode);
            GameObject gameObject = getInstanceFromPoolEvent2.Instance.gameObject;
            gameObject.transform.position = e.Position;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
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