using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class PenetrationTargetCollectorSystem : AbstractPenetrationTargetCollectorSystem {
        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode,
            [JoinByTank] SingleNode<TankCollidersComponent> tankCollidersNode) {
            TargetingData targetingData = evt.TargetingData;
            CollectTargetsOnDirectionsByTargetingColliders(targetingData, tankCollidersNode.component);
        }

        public class TankNode : Node {
            public TankCollidersComponent tankColliders;
            public TankGroupComponent tankGroup;
        }

        public class TargetCollectorNode : Node {
            public PenetrationTargetCollectorComponent penetrationTargetCollector;
        }
    }
}