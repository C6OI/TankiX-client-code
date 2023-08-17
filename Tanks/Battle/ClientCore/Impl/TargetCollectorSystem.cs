using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TargetCollectorSystem : AbstractTargetCollectorSystem {
        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode,
            [JoinByTank] SingleNode<TankCollidersComponent> tankCollidersNode) {
            TargetingData targetingData = evt.TargetingData;

            CollectTargetsWithExclusion(targetingData,
                tankCollidersNode.component.TargetingColliders,
                TargetValidator,
                GetLocalHitPointByPhysics,
                targetCollectorNode.targetCollector.Mask);
        }

        static bool TargetValidator(Entity target, RaycastHit hitInfo, bool isInsideAttack) =>
            target.HasComponent<TankActiveStateComponent>();

        public class TargetCollectorNode : Node {
            public TankGroupComponent tankGroup;

            public TargetCollectorComponent targetCollector;
        }
    }
}