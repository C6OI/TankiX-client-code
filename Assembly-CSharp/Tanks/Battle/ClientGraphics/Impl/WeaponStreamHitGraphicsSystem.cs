using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class WeaponStreamHitGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, WeaponStreamHitGraphicsEffectInitNode weapon) {
            WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect = weapon.weaponStreamHitGraphicsEffect;
            weaponStreamHitGraphicsEffect.Init(weapon.muzzlePoint.Current);
            weapon.Entity.AddComponent<WeaponStreamHitGraphicsEffectReadyComponent>();
        }

        [OnEventFire]
        public void StopHitEffect(NodeRemoveEvent evt, WeaponStreamHitGraphicsNode weapon) {
            WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect = weapon.weaponStreamHitGraphicsEffect;

            if ((bool)weaponStreamHitGraphicsEffect.HitStatic) {
                weaponStreamHitGraphicsEffect.HitStatic.Stop(true);
            }

            if (weaponStreamHitGraphicsEffect.HitTarget != null) {
                weaponStreamHitGraphicsEffect.HitTarget.Stop(true);
            }

            if (weaponStreamHitGraphicsEffect.HitStaticLight != null) {
                weaponStreamHitGraphicsEffect.HitStaticLight.enabled = false;
            }

            if (weaponStreamHitGraphicsEffect.HitTargetLight != null) {
                weaponStreamHitGraphicsEffect.HitTargetLight.enabled = false;
            }
        }

        [OnEventComplete]
        public void UpdateHitEffect(UpdateEvent evt, WeaponStreamHitGraphicsNode weapon) {
            WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect = weapon.weaponStreamHitGraphicsEffect;
            StreamHitComponent streamHit = weapon.streamHit;

            if (streamHit.StaticHit != null) {
                weaponStreamHitGraphicsEffect.HitStatic.transform.position = streamHit.StaticHit.Position + streamHit.StaticHit.Normal * weaponStreamHitGraphicsEffect.HitOffset;
                weaponStreamHitGraphicsEffect.HitStatic.transform.rotation = Quaternion.LookRotation(streamHit.StaticHit.Normal);
                weaponStreamHitGraphicsEffect.HitStatic.Play(true);
                weaponStreamHitGraphicsEffect.HitStaticLight.enabled = true;
            } else if (streamHit.TankHit != null && weapon.Entity.HasComponent<StreamHitTargetLoadedComponent>()) {
                Entity entity = streamHit.TankHit.Entity;
                UpdateWeaponStreamHitGraphicsByTargetTankEvent updateWeaponStreamHitGraphicsByTargetTankEvent = new();
                updateWeaponStreamHitGraphicsByTargetTankEvent.HitTargetParticleSystem = weaponStreamHitGraphicsEffect.HitTarget;
                updateWeaponStreamHitGraphicsByTargetTankEvent.HitTargetLight = weaponStreamHitGraphicsEffect.HitTargetLight;
                updateWeaponStreamHitGraphicsByTargetTankEvent.TankHit = streamHit.TankHit;
                updateWeaponStreamHitGraphicsByTargetTankEvent.HitOffset = weaponStreamHitGraphicsEffect.HitOffset;
                ScheduleEvent(updateWeaponStreamHitGraphicsByTargetTankEvent, entity);
            }
        }

        [OnEventFire]
        public void UpdateHitEffect(UpdateWeaponStreamHitGraphicsByTargetTankEvent evt, HullNode tank) {
            GameObject hullInstance = tank.hullInstance.HullInstance;
            Vector3 vector = hullInstance.transform.TransformPoint(evt.TankHit.LocalHitPoint);
            Quaternion rotation = Quaternion.LookRotation(evt.TankHit.HitDirection);
            evt.HitTargetParticleSystem.transform.position = vector - evt.TankHit.HitDirection * evt.HitOffset;
            evt.HitTargetParticleSystem.transform.rotation = rotation;
            evt.HitTargetParticleSystem.Play(true);
            evt.HitTargetLight.enabled = true;
        }

        public class WeaponStreamHitGraphicsEffectInitNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect;
        }

        public class WeaponStreamHitGraphicsNode : Node {
            public StreamHitComponent streamHit;

            public WeaponStreamHitGraphicsEffectComponent weaponStreamHitGraphicsEffect;
            public WeaponStreamHitGraphicsEffectReadyComponent weaponStreamHitGraphicsEffectReady;
        }

        public class HullNode : Node {
            public HullInstanceComponent hullInstance;

            public NewHolyshieldEffectComponent newHolyshieldEffect;
        }
    }
}