using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class HitTarget {
        public Entity Entity { get; set; }

        public Vector3 LocalHitPoint { get; set; }

        public Vector3 TargetPosition { get; set; }

        public float HitDistance { get; set; }

        public Vector3 HitDirection { get; set; }

        public override string ToString() =>
            string.Format("Entity: {0}, LocalHitPoint: {1}, TargetPosition: {2}, HitDistance: {3}, HitDirection: {4}",
                Entity,
                LocalHitPoint,
                TargetPosition,
                HitDistance,
                HitDirection);
    }
}