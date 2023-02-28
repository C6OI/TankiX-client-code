using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HitExplosionGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void CreateExplosionOnEachTarget(HitEvent evt, WeaponNode weapon) {
            HitExplosionGraphicsComponent hitExplosionGraphics = weapon.hitExplosionGraphics;
            Vector3 fireDirectionWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetFireDirectionWorld();

            foreach (HitTarget target in evt.Targets) {
                ExplosionEvent explosionEvent = new();
                explosionEvent.ExplosionOffset = -fireDirectionWorld * hitExplosionGraphics.ExplosionOffset;
                explosionEvent.HitDirection = target.HitDirection;
                explosionEvent.Asset = hitExplosionGraphics.ExplosionAsset;
                explosionEvent.Duration = hitExplosionGraphics.ExplosionDuration;
                explosionEvent.Target = target;
                ExplosionEvent eventInstance = explosionEvent;
                ScheduleEvent(eventInstance, target.Entity);
            }

            if (evt.StaticHit != null) {
                Vector3 position = evt.StaticHit.Position - fireDirectionWorld * hitExplosionGraphics.ExplosionOffset;
                DrawExplosionEffect(position, evt.StaticHit.Normal, hitExplosionGraphics.ExplosionAsset, hitExplosionGraphics.ExplosionDuration, weapon);
            }
        }

        [OnEventFire]
        public void CreateExplosionEffect(ExplosionEvent evt, TankNode tank) {
            Transform transform = tank.tankVisualRoot.transform;
            Vector3 position = transform.TransformPoint(evt.Target.LocalHitPoint) + evt.ExplosionOffset;
            DrawExplosionEffect(position, evt.ExplosionOffset, evt.Asset, evt.Duration, tank);
        }

        [OnEventFire]
        public void CreateBlockedExplosionEffect(BaseShotEvent evt, BlockedWeaponNode node) {
            HitExplosionGraphicsComponent hitExplosionGraphics = node.hitExplosionGraphics;
            WeaponBlockedComponent weaponBlocked = node.weaponBlocked;
            Vector3 position = weaponBlocked.BlockPoint - evt.ShotDirection * hitExplosionGraphics.ExplosionOffset;

            if (hitExplosionGraphics.UseForBlockedWeapon) {
                DrawExplosionEffect(position, weaponBlocked.BlockNormal, hitExplosionGraphics.ExplosionAsset, hitExplosionGraphics.ExplosionDuration, node);
            }
        }

        void DrawExplosionEffect(Vector3 position, Vector3 dir, GameObject asset, float duration, Node entity) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = asset;
            getInstanceFromPoolEvent.AutoRecycleTime = duration;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, entity);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            instance.position = position;
            instance.rotation = Quaternion.Euler(dir);
            instance.gameObject.SetActive(true);
            instance.gameObject.GetComponent<ParticleSystem>().Play(true);
        }

        public class WeaponNode : Node {
            public HitExplosionGraphicsComponent hitExplosionGraphics;

            public MuzzlePointComponent muzzlePoint;
            public WeaponComponent weapon;
        }

        public class BlockedWeaponNode : Node {
            public HitExplosionGraphicsComponent hitExplosionGraphics;
            public WeaponComponent weapon;

            public WeaponBlockedComponent weaponBlocked;
        }

        public class TankNode : Node {
            public TankComponent tank;

            public TankVisualRootComponent tankVisualRoot;
        }
    }
}