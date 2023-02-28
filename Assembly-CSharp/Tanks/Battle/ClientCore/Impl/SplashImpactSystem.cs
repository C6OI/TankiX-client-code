using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SplashImpactSystem : AbstractImpactSystem {
        [OnEventFire]
        public void CalculateAndSendSplashImpactEffect(SelfSplashHitEvent evt, SplashImpactNode weapon, [JoinByTank] TankPhysicsNode tank) {
            CalculateAndSendSplashImpactEffectByBaseEvent(evt.SplashTargets, evt.StaticHit, evt.Targets, weapon, tank);
        }

        [OnEventFire]
        public void CalculateAndSendSplashImpactEffect(RemoteSplashHitEvent evt, SplashImpactNode weapon, [JoinByTank] TankPhysicsNode tank) {
            CalculateAndSendSplashImpactEffectByBaseEvent(evt.SplashTargets, evt.StaticHit, evt.Targets, weapon, tank);
        }

        void CalculateAndSendSplashImpactEffectByBaseEvent(List<HitTarget> splashTargets, StaticHit staticHit, List<HitTarget> targets, SplashImpactNode weapon, TankPhysicsNode tank) {
            SplashImpactComponent splashImpact = weapon.splashImpact;
            SplashWeaponComponent splashWeapon = weapon.splashWeapon;
            Vector3 vector = staticHit == null ? targets[0].TargetPosition : staticHit.Position;
            float num = 1f;

            if (weapon.Entity.HasComponent<DamageWeakeningByDistanceComponent>()) {
                DamageWeakeningByDistanceComponent component = weapon.Entity.GetComponent<DamageWeakeningByDistanceComponent>();
                Vector3 position = tank.rigidBody.Rigidbody.position;
                float magnitude = (position - vector).magnitude;
                num = GetImpactWeakeningByRange(magnitude, component);
            }

            foreach (HitTarget splashTarget in splashTargets) {
                float hitDistance = splashTarget.HitDistance;
                float splashImpactWeakeningByRange = GetSplashImpactWeakeningByRange(hitDistance, splashWeapon);
                ImpactEvent impactEvent = new();
                Vector3 vector2 = Vector3.Normalize(splashTarget.HitDirection) * splashImpact.ImpactForce * WeaponConstants.WEAPON_FORCE_MULTIPLIER;
                impactEvent.Force = vector2 * num * splashImpactWeakeningByRange;
                impactEvent.LocalHitPoint = splashTarget.LocalHitPoint;
                impactEvent.WeakeningCoeff = splashImpactWeakeningByRange;
                NewEvent(impactEvent).AttachAll(weapon.Entity, splashTarget.Entity).Schedule();
            }
        }

        float GetSplashImpactWeakeningByRange(float distance, SplashWeaponComponent splashWeapon) {
            float radiusOfMaxSplashDamage = splashWeapon.RadiusOfMaxSplashDamage;
            float radiusOfMinSplashDamage = splashWeapon.RadiusOfMinSplashDamage;
            float minSplashDamagePercent = splashWeapon.MinSplashDamagePercent;

            if (distance < radiusOfMaxSplashDamage) {
                return 1f;
            }

            if (distance > radiusOfMinSplashDamage) {
                return 0f;
            }

            return 0.01f * (minSplashDamagePercent + (radiusOfMinSplashDamage - distance) * (100f - minSplashDamagePercent) / (radiusOfMinSplashDamage - radiusOfMaxSplashDamage));
        }

        public class SplashImpactNode : Node {
            public SplashImpactComponent splashImpact;

            public SplashWeaponComponent splashWeapon;

            public TankGroupComponent tankGroup;
        }

        public class TankPhysicsNode : Node {
            public RigidbodyComponent rigidBody;

            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }
    }
}