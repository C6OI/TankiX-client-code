using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class AbstractImpactSystem : ECSSystem {
        const float DEFAULT_WEAKENING_COEFF = 1f;

        const float PERCENT_MULTIPLIER = 0.01f;

        protected void PrepareImpactForHitTarget(Entity weaponDamager, HitTarget target, float maxImpactForce, float weakeningCoeff = 1f) {
            ImpactEvent impactEvent = new();
            Vector3 vector = Vector3.Normalize(target.HitDirection) * maxImpactForce * WeaponConstants.WEAPON_FORCE_MULTIPLIER;
            impactEvent.Force = vector * weakeningCoeff;
            impactEvent.LocalHitPoint = target.LocalHitPoint;
            impactEvent.WeakeningCoeff = weakeningCoeff;
            NewEvent(impactEvent).AttachAll(target.Entity, weaponDamager).Schedule();
        }

        protected float GetImpactWeakeningByRange(float distance, DamageWeakeningByDistanceComponent weakeningConfig) {
            float minDamagePercent = weakeningConfig.MinDamagePercent;
            float radiusOfMaxDamage = weakeningConfig.RadiusOfMaxDamage;
            float radiusOfMinDamage = weakeningConfig.RadiusOfMinDamage;
            float num = radiusOfMinDamage - radiusOfMaxDamage;

            if (num <= 0f) {
                return 1f;
            }

            if (distance <= radiusOfMaxDamage) {
                return 1f;
            }

            if (distance >= radiusOfMinDamage) {
                return 0.01f * minDamagePercent;
            }

            return 0.01f * (minDamagePercent + (radiusOfMinDamage - distance) * (100f - minDamagePercent) / num);
        }

        protected void ApplyImpactByTargetWeakening(Entity weaponDamager, List<HitTarget> targets, float forceVal, float weakeningByTargetPercent) {
            float num = 1f;
            float num2 = weakeningByTargetPercent * 0.01f;
            int count = targets.Count;

            for (int i = 0; i < count; i++) {
                HitTarget hitTarget = targets[i];
                ImpactEvent impactEvent = new();
                Vector3 vector = Vector3.Normalize(hitTarget.HitDirection) * forceVal * WeaponConstants.WEAPON_FORCE_MULTIPLIER;
                impactEvent.Force = vector * num;
                impactEvent.LocalHitPoint = hitTarget.LocalHitPoint;
                impactEvent.WeakeningCoeff = num;
                num *= num2;
                NewEvent(impactEvent).AttachAll(weaponDamager, hitTarget.Entity).Schedule();
            }
        }
    }
}