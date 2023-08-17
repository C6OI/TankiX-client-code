using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftImpactSystem : AbstractImpactSystem {
        [OnEventFire]
        public void ApplyBaseHitImpact(HitEvent evt, ShaftAimingImpactNode weapon) {
            ImpactComponent impact = weapon.impact;
            List<HitTarget> targets = evt.Targets;
            int count = targets.Count;
            float impactForce = impact.ImpactForce;

            for (int i = 0; i < count; i++) {
                HitTarget target = targets[i];
                PrepareImpactForHitTarget(target, impactForce);
            }
        }

        [OnEventFire]
        public void MakeAimingHitImpact(SelfShaftAimingHitEvent evt, ShaftAimingImpactNode weapon) =>
            MakeImpactOnAnyAimingShot(evt.HitPower, evt.Targets, weapon);

        [OnEventFire]
        public void MakeAimingHitImpact(RemoteShaftAimingHitEvent evt, ShaftAimingImpactNode weapon) =>
            MakeImpactOnAnyAimingShot(evt.HitPower, evt.Targets, weapon);

        void MakeImpactOnAnyAimingShot(float aimingHitPower, List<HitTarget> targets, ShaftAimingImpactNode weapon) {
            float impactForce = weapon.impact.ImpactForce;
            float maxImpactForce = weapon.shaftAimingImpact.MaxImpactForce;
            float maxImpactForce2 = (maxImpactForce - impactForce) * aimingHitPower;
            int count = targets.Count;

            for (int i = 0; i < count; i++) {
                HitTarget target = targets[i];
                PrepareImpactForHitTarget(target, maxImpactForce2);
            }
        }

        public class ShaftAimingImpactNode : Node {
            public ImpactComponent impact;
            public ShaftAimingImpactComponent shaftAimingImpact;
        }
    }
}