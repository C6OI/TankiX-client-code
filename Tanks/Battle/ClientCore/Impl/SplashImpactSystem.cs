using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SplashImpactSystem : AbstractImpactSystem {
        [OnEventFire]
        public void CalculateAndSendSplashImpactEffect(SelfSplashHitEvent evt, SplashImpactNode weapon,
            [JoinByTank] TankPhysicsNode tank) =>
            CalculateAndSendSplashImpactEffectByBaseEvent(evt.SplashTargets, evt.StaticHit, evt.Targets, weapon, tank);

        [OnEventFire]
        public void CalculateAndSendSplashImpactEffect(RemoteSplashHitEvent evt, SplashImpactNode weapon,
            [JoinByTank] TankPhysicsNode tank) =>
            CalculateAndSendSplashImpactEffectByBaseEvent(evt.SplashTargets, evt.StaticHit, evt.Targets, weapon, tank);

        void CalculateAndSendSplashImpactEffectByBaseEvent(List<HitTarget> splashTargets, StaticHit staticHit,
            List<HitTarget> targets, SplashImpactNode weapon, TankPhysicsNode tank) {
            DamageWeakeningByDistanceComponent damageWeakeningByDistance = weapon.damageWeakeningByDistance;
            SplashImpactComponent splashImpact = weapon.splashImpact;
            SplashWeaponComponent splashWeapon = weapon.splashWeapon;
            Vector3 position = tank.rigidBody.Rigidbody.position;
            Vector3 vector = staticHit == null ? targets[0].TargetPosition : staticHit.Position;
            float magnitude = (position - vector).magnitude;
            float impactWeakeningByRange = GetImpactWeakeningByRange(magnitude, damageWeakeningByDistance);

            foreach (HitTarget splashTarget in splashTargets) {
                float hitDistance = splashTarget.HitDistance;
                float splashImpactWeakeningByRange = GetSplashImpactWeakeningByRange(hitDistance, splashWeapon);
                ImpactEvent impactEvent = new();

                Vector3 vector2 = Vector3.Normalize(splashTarget.HitDirection) *
                                  splashImpact.ImpactForce *
                                  WeaponConstants.WEAPON_FORCE_MULTIPLIER;

                impactEvent.Force = vector2 * impactWeakeningByRange * splashImpactWeakeningByRange;
                impactEvent.LocalHitPoint = splashTarget.LocalHitPoint;
                ScheduleEvent(impactEvent, splashTarget.Entity);
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

            return 0.01f *
                   (minSplashDamagePercent +
                    (radiusOfMinSplashDamage - distance) *
                    (100f - minSplashDamagePercent) /
                    (radiusOfMinSplashDamage - radiusOfMaxSplashDamage));
        }

        public class SplashImpactNode : Node {
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;
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