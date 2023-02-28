using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class DiscreteImpactSystem : AbstractImpactSystem {
        [OnEventFire]
        public void PrepareImpact(HitEvent evt, ImpactWeakeningNode weapon) {
            PrepareImpactByBaseHitEvent(evt, weapon);
        }

        [OnEventFire]
        public void Impact(ImpactEvent evt, TankNode tank) {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            Vector3 position = MathUtil.LocalPositionToWorldPosition(evt.LocalHitPoint, rigidbody.gameObject);
            rigidbody.AddForceAtPositionSafe(evt.Force, position);
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
                PrepareImpactForHitTarget(weapon.Entity, hitTarget, impactForce, impactWeakeningByRange);
            }
        }

        public class ImpactWeakeningNode : Node {
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;

            public DiscreteWeaponComponent discreteWeapon;
            public ImpactComponent impact;
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;
            public TankComponent tank;
        }
    }
}