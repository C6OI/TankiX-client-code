using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftShotAnimationStates {
        public class ShaftShotAnimationIdleState : Node {
            public ShaftShotAnimationIdleStateComponent shaftShotAnimationIdleState;
        }

        public class ShaftShotAnimationBounceState : Node {
            public ShaftShotAnimationBounceStateComponent shaftShotAnimationBounceState;
        }

        public class ShaftShotAnimationCooldownState : Node {
            public ShaftShotAnimationCooldownStateComponent shaftShotAnimationCooldownState;
        }
    }
}