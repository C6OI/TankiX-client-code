using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class HammerTargetCollectorSystem : ECSSystem {
        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode, [JoinByTank] SingleNode<TankComponent> tank) {
            TargetingData targetingData = evt.TargetingData;
            List<DirectionData> directions = targetingData.Directions;
            int count = targetingData.Directions.Count;

            for (int i = 0; i < count; i++) {
                DirectionData directionData = directions[i];
                directionData.Clean();
                CollectPelletTargets(targetingData, directionData, targetCollectorNode);
            }
        }

        void CollectPelletTargets(TargetingData targetingData, DirectionData directionData, TargetCollectorNode targetCollectorNode) {
            HammerPelletConeComponent hammerPelletCone = targetCollectorNode.hammerPelletCone;
            MuzzlePointComponent muzzlePoint = targetCollectorNode.muzzlePoint;
            Vector3 dir = directionData.Dir;
            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(dir);
            Vector3[] randomDirections = PelletDirectionsCalculator.GetRandomDirections(hammerPelletCone, muzzlePoint.Current.rotation, localDirection);

            for (int i = 0; i < randomDirections.Length; i++) {
                directionData.Dir = randomDirections[i];
                targetCollectorNode.hammerTargetCollector.Collect(targetingData.FullDistance, directionData);
            }

            directionData.Dir = dir;
        }

        public class TargetCollectorNode : Node {
            public HammerPelletConeComponent hammerPelletCone;

            public HammerTargetCollectorComponent hammerTargetCollector;

            public MuzzlePointComponent muzzlePoint;
            public TankGroupComponent tankGroup;
        }
    }
}