using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ImpactWeakeningByTargetSystem : AbstractImpactSystem {
        [OnEventFire]
        public void PrepareImpactOnHit(HitEvent evt, ImpactNode weapon) {
            ImpactComponent impact = weapon.impact;
            DamageWeakeningByTargetComponent damageWeakeningByTarget = weapon.damageWeakeningByTarget;
            ApplyImpactByTargetWeakening(weapon.Entity, evt.Targets, impact.ImpactForce, damageWeakeningByTarget.DamagePercent);
        }

        public class ImpactNode : Node {
            public DamageWeakeningByTargetComponent damageWeakeningByTarget;
            public ImpactComponent impact;
        }
    }
}