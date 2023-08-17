using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class DiscreteImpactSystem : AbstractImpactSystem {
        [OnEventFire]
        public void PrepareImpact(HitEvent evt, ImpactWeakeningNode weapon) => PrepareImpactByBaseHitEvent(evt, weapon);

        [OnEventFire]
        public void Impact(ImpactEvent evt, SingleNode<RigidbodyComponent> tank) {
            Rigidbody rigidbody = tank.component.Rigidbody;
            Vector3 position = MathUtil.LocalPositionToWorldPosition(evt.LocalHitPoint, rigidbody.gameObject);
            rigidbody.AddForceAtPosition(evt.Force, position);
        }

        void PrepareImpactByBaseHitEvent(HitEvent evt, ImpactWeakeningNode weapon) {
            ImpactComponent impact = weapon.impact;
            DamageWeakeningByDistanceComponent damageWeakeningByDistance = weapon.damageWeakeningByDistance;
            List<HitTarget> targets = evt.Targets;
            int count = targets.Count;
            float impactForce = impact.ImpactForce;

            for (int i = 0; i < count; i++) {
                HitTarget hitTarget = targets[i];
                float hitDistance = hitTarget.HitDistance;
                float impactWeakeningByRange = GetImpactWeakeningByRange(hitDistance, damageWeakeningByDistance);
                PrepareImpactForHitTarget(hitTarget, impactForce, impactWeakeningByRange);
            }
        }

        public class ImpactWeakeningNode : Node {
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;

            public DiscreteWeaponComponent discreteWeapon;
            public ImpactComponent impact;
        }
    }
}