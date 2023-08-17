using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BulletTargetCollectorSystem : AbstractTargetCollectorSystem {
        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode,
            [JoinByTank] SingleNode<TankCollidersComponent> tankCollidersNode) {
            TargetingData targetingData = evt.TargetingData;

            if (targetCollectorNode.bulletTargetCollector.UseRaycastExclusion) {
                CollectTargetsWithExclusion(targetingData,
                    tankCollidersNode.component.TargetingColliders,
                    TargetValidator,
                    GetLocalHitPointByPhysics,
                    LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS);
            } else {
                CollectTargets(targetingData,
                    TargetValidator,
                    GetLocalHitPointByPhysics,
                    LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS);
            }

            ScheduleEvent(new TargetingEvaluateEvent(targetingData), targetCollectorNode);
        }

        bool TargetValidator(Entity target, RaycastHit hitInfo, bool isInsideAttack) => true;

        public class TargetCollectorNode : Node {
            public BulletTargetCollectorComponent bulletTargetCollector;
            public TankGroupComponent tankGroup;
        }
    }
}