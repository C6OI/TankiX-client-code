using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class VulcanImpactSystem : AbstractImpactSystem {
        [OnEventFire]
        public void PrepareImpactOnShot(FixedUpdateEvent evt, ImpactNode weapon) {
            ImpactComponent impact = weapon.impact;
            DamageWeakeningByDistanceComponent damageWeakeningByDistance = weapon.damageWeakeningByDistance;
            HitTarget tankHit = weapon.streamHit.TankHit;
            float deltaTime = evt.DeltaTime;
            VulcanImpactEvent vulcanImpactEvent = new();
            float hitDistance = tankHit.HitDistance;

            Vector3 vector = Vector3.Normalize(tankHit.HitDirection) *
                             impact.ImpactForce *
                             WeaponConstants.WEAPON_FORCE_MULTIPLIER *
                             deltaTime;

            vulcanImpactEvent.Force = vector * GetImpactWeakeningByRange(hitDistance, damageWeakeningByDistance);
            vulcanImpactEvent.LocalHitPoint = tankHit.LocalHitPoint;
            ScheduleEvent(vulcanImpactEvent, tankHit.Entity);
        }

        [OnEventFire]
        public void ApplyVulcanImpact(VulcanImpactEvent evt, TankNode tank) {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            Vector3 pos = MathUtil.LocalPositionToWorldPosition(evt.LocalHitPoint, rigidbody.gameObject);
            TrackComponent track = tank.track;
            TankFallingComponent tankFalling = tank.tankFalling;
            VulcanPhysicsUtils.ApplyVulcanForce(evt.Force, rigidbody, pos, tankFalling, track);
        }

        public class ImpactNode : Node {
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;
            public ImpactComponent impact;

            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;

            public TankFallingComponent tankFalling;

            public TrackComponent track;
        }
    }
}