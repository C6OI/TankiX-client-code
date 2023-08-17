using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingInputContextSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void SwitchContextToAiming(NodeAddedEvent evt, ShaftAimingWorkActivationStateNode weapon,
            [JoinAll] [Mandatory] SingleNode<BattleHUDESMComponent> battleHUD) {
            InputManager.DeactivateContext(BasicContexts.BATTLE_CONTEXT);
            InputManager.ClearActions();
            InputManager.ActivateContext(ShaftAimingContexts.AIMING_TARGETING);
            battleHUD.component.Esm.ChangeState<BattleHUDStates.ShaftAimingState>();
        }

        [OnEventFire]
        public void SwitchContextToBattle(NodeAddedEvent evt, AimingIdleStateNode weapon,
            [JoinAll] SingleNode<BattleHUDESMComponent> battleHUD) {
            InputManager.DeactivateContext(ShaftAimingContexts.AIMING_TARGETING);
            InputManager.ClearActions();
            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
            battleHUD.component.Esm.ChangeState<BattleHUDStates.ActionsState>();
        }

        public class ShaftAimingWorkActivationStateNode : Node {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }

        public class AimingIdleStateNode : Node {
            public ShaftIdleStateComponent shaftIdleState;

            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }
    }
}