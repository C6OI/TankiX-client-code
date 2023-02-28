using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankOutlineMapEffectStates {
        public class IdleState : Node {
            public TankOutlineMapEffectIdleStateComponent tankOutlineMapEffectIdleState;
        }

        public class ActivationState : Node {
            public TankOutlineMapEffectActivationStateComponent tankOutlineMapEffectActivationState;
        }

        public class WorkingState : Node {
            public TankOutlineMapEffectWorkingStateComponent tankOutlineMapEffectWorkingState;
        }

        public class BlinkerState : Node {
            public TankOutlineMapEffectBlinkerStateComponent tankOutlineMapEffectBlinkerState;
        }

        public class DeactivationState : Node {
            public TankOutlineMapEffectDeactivationStateComponent tankOutlineMapEffectDeactivationState;
        }
    }
}