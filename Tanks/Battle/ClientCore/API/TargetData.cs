using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class TargetData {
        public Entity Entity { get; set; }

        public Vector3 HitPoint { get; set; }

        public Vector3 LocalHitPoint { get; set; }

        public Vector3 TargetPosition { get; set; }

        public Vector3 HitDirection { get; set; }

        public float HitDistance { get; set; }

        public float Priority { get; set; }

        public bool ValidTarget { get; set; } = true;

        public int PriorityWeakeningCount { get; set; }

        public TargetData Init(Entity entity = null) {
            Entity = entity;
            HitPoint = Vector3.zero;
            LocalHitPoint = Vector3.zero;
            TargetPosition = Vector3.zero;
            HitDirection = Vector3.zero;
            Priority = 0f;
            ValidTarget = true;
            return this;
        }

        public override string ToString() => string.Format(
            "Entity: {0}, HitPoint: {1}, LocalHitPoint: {2}, TargetPosition: {3}, HitDirection: {4}, HitDistance: {5}, Priority: {6}, ValidTarget: {7}, PriorityWeakeningCount: {8}",
            Entity,
            HitPoint,
            LocalHitPoint,
            TargetPosition,
            HitDirection,
            HitDistance,
            Priority,
            ValidTarget,
            PriorityWeakeningCount);
    }
}