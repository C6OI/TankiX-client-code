using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankAnimationSystem : ECSSystem {
        static readonly string INACTIVE_STATE_TAG = "Inaction";

        [OnEventFire]
        public void SetNewAnimationAsNotPrepared(NodeAddedEvent evt, [Combine] AnimationNode animationNode,
            [JoinByTank] [Context] ActivatedTankNode tank) =>
            animationNode.Entity.AddComponent<AnimationPreparedComponent>();

        public class AnimationNode : Node {
            public AnimationComponent animation;

            public TankGroupComponent tankGroup;
        }

        public class ActivatedTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public TankGroupComponent tankGroup;
        }

        public class PreparedAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
        }
    }
}