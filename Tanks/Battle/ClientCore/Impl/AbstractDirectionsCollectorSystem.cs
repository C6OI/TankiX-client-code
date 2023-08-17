using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public abstract class AbstractDirectionsCollectorSystem : ECSSystem {
        const float MAXIMUM_RAYS_PER_DEG = 8f;

        const float MIN_TANK_SIZE = 0.9f;

        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        protected void CollectSectorDirections(Vector3 origin, Vector3 dir, Vector3 rotationAxis, float angleStep,
            int numRays, TargetingData targetingData) {
            float num = 0f;

            for (int i = 1; i <= numRays; i++) {
                num += angleStep;
                Vector3 dir2 = Quaternion.AngleAxis(0f - num, rotationAxis) * dir;
                CollectDirection(origin, dir2, Mathf.Abs(num), targetingData);
            }
        }

        protected static DirectionData CollectDirection(Vector3 origin, Vector3 dir, float angle,
            TargetingData targetingData) {
            DirectionData directionData = BattleCache.directionData.GetInstance().Init(origin, dir, angle);
            targetingData.Directions.Add(directionData);
            return directionData;
        }

        public static void CollectExtraDirection(Vector3 origin, Vector3 dir, float angle, TargetingData targetingData) =>
            CollectDirection(origin, dir, angle, targetingData).Extra = true;

        protected float CalculateRaysPerDeg(float distance) =>
            (float)Math.Min(8.0, 1.0 / (2.0 * Math.Atan(0.9f / (2f * distance))));
    }
}