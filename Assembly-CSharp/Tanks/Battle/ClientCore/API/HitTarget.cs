using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class HitTarget {
        public Entity Entity { get; set; }

        public Entity IncarnationEntity { get; set; }

        public Vector3 LocalHitPoint { get; set; }

        public Vector3 TargetPosition { get; set; }

        public float HitDistance { get; set; }

        public Vector3 HitDirection { get; set; }
    }
}