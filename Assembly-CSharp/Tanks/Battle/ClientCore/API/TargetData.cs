using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class TargetData {
        public Entity TargetEntity { get; private set; }

        public Entity TargetIncorantionEntity { get; private set; }

        public Vector3 HitPoint { get; set; }

        public Vector3 LocalHitPoint { get; set; }

        public Vector3 TargetPosition { get; set; }

        public Vector3 HitDirection { get; set; }

        public float HitDistance { get; set; }

        public float Priority { get; set; }

        public bool ValidTarget { get; set; } = true;

        public int PriorityWeakeningCount { get; set; }

        public TargetData Init() {
            Init(null, null);
            return this;
        }

        public TargetData Init(Entity targetEntity, Entity targetIncarnationEntity) {
            TargetEntity = targetEntity;
            TargetIncorantionEntity = targetIncarnationEntity;
            HitPoint = Vector3.zero;
            LocalHitPoint = Vector3.zero;
            TargetPosition = Vector3.zero;
            HitDirection = Vector3.zero;
            Priority = 0f;
            ValidTarget = true;
            return this;
        }

        public void SetTarget(Entity targetEntity, Entity targetIncarnationEntity) {
            TargetEntity = targetEntity;
            TargetIncorantionEntity = targetIncarnationEntity;
        }

        public override string ToString() =>
            string.Format(
                "Entity: {0}, HitPoint: {1}, LocalHitPoint: {2}, TargetPosition: {3}, HitDirection: {4}, HitDistance: {5}, Priority: {6}, ValidTarget: {7}, PriorityWeakeningCount: {8}",
                TargetEntity, HitPoint, LocalHitPoint, TargetPosition, HitDirection, HitDistance, Priority, ValidTarget, PriorityWeakeningCount);
    }
}