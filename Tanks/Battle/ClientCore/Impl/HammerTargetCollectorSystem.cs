using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class HammerTargetCollectorSystem : AbstractTargetCollectorSystem {
        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode,
            [JoinByTank] SingleNode<TankCollidersComponent> tankCollidersNode) {
            TargetingData targetingData = evt.TargetingData;
            HammerPelletConeComponent hammerPelletCone = targetCollectorNode.hammerPelletCone;
            RaycastExclude raycastExclude = new(tankCollidersNode.component.TargetingColliders);

            try {
                foreach (DirectionData direction in targetingData.Directions) {
                    CollectPelletTargets(targetingData, direction, hammerPelletCone, targetCollectorNode.muzzlePoint);
                }
            } finally {
                raycastExclude.Dispose();
            }
        }

        void CollectPelletTargets(TargetingData targetingData, DirectionData directionData, HammerPelletConeComponent config,
            MuzzlePointComponent muzzlePoint) {
            Vector3 dir = directionData.Dir;
            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(dir);

            Vector3[] randomDirections = PelletDirectionsCalculator.GetRandomDirections(config,
                muzzlePoint.Current.rotation,
                localDirection,
                Time.frameCount);

            for (int i = 0; i < randomDirections.Length; i++) {
                directionData.Dir = randomDirections[i];

                FindTargetOnDirection(targetingData,
                    directionData,
                    TargetValidator,
                    GetLocalHitPointByPhysics,
                    LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS);
            }

            directionData.Dir = dir;
        }

        bool TargetValidator(Entity target, RaycastHit hitInfo, bool isInsideAttack) =>
            target.HasComponent<TankActiveStateComponent>();

        public class TargetCollectorNode : Node {
            public HammerPelletConeComponent hammerPelletCone;

            public MuzzlePointComponent muzzlePoint;
            public TankGroupComponent tankGroup;
        }
    }
}