using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class ImpactEvent : Event {
        Vector3 force;
        Vector3 localHitPoint;

        float weakeningCoeff;

        public Vector3 LocalHitPoint {
            get => localHitPoint;
            set => localHitPoint = !ValidateImpactData(value) ? Vector3.zero : value;
        }

        public Vector3 Force {
            get => force;
            set => force = !ValidateImpactData(value) ? Vector3.zero : value;
        }

        public float WeakeningCoeff {
            get => weakeningCoeff;
            set => weakeningCoeff = !PhysicsUtil.IsValidFloat(value) ? 0f : value;
        }

        public static bool ValidateImpactData(Vector3 force) => PhysicsUtil.IsValidVector3(force);
    }
}