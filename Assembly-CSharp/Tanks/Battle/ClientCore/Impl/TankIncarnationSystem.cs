using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankIncarnationSystem : ECSSystem {
        [OnEventFire]
        public void AcceptTankIncarnation(NodeAddedEvent e, SelfTankNode selfTank, [JoinByTank] [Context] TankIncarnationNode tankIncarnation) {
            tankIncarnation.Entity.AddComponent<TankClientIncarnationComponent>();
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}