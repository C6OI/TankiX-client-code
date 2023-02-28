using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleInputContextSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void OnExitBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser) {
            SwitchContextToActions();
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
        }

        [OnEventFire]
        public void OnFocus(ApplicationFocusEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) {
            if (e.IsFocused) {
                SwitchContextToActions();
                ScheduleEvent(new CheckMouseEvent(), selfBattleUser.Entity);
            }
        }

        [OnEventFire]
        public void UpdateToScore(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE)) {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [OnEventFire]
        public void UpdateToScore(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleShaftAimingStateComponent> battleShaftAimingState) {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE)) {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [OnEventComplete]
        public void Update(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleShaftAimingStateComponent> battleShaftAimingState) {
            if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE)) {
                ScheduleEvent(new CheckMouseEvent(), selfBattleUser.Entity);
            }
        }

        [OnEventComplete]
        public void Update(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) {
            if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE)) {
                ScheduleEvent(new CheckMouseEvent(), selfBattleUser.Entity);
            }
        }

        [OnEventFire]
        public void SetBattleChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState) {
            SwitchContextToChat();
        }

        [OnEventFire]
        public void SetBattleActionsState(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState) {
            SwitchContextToActions();
        }

        void SwitchContextToActions() {
            InputManager.DeactivateContext(BattleChatContexts.BATTLE_CHAT);
            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
        }

        void SwitchContextToChat() {
            InputManager.DeactivateContext(BasicContexts.BATTLE_CONTEXT);
            InputManager.ActivateContext(BattleChatContexts.BATTLE_CHAT);
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
        }

        [OnEventFire]
        public void DeactivateMouseInSpectator(NodeAddedEvent e, SingleNode<SpectatorCameraComponent> spectator) {
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
        }

        [OnEventFire]
        public void CheckMouseState(CheckMouseEvent e, SelfBattleUser selfBattleUser, Optional<SingleNode<SpectatorCameraComponent>> spectator) {
            if (spectator.IsPresent()) {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            } else if (selfBattleUser.mouseControlStateHolder.MouseControlAllowed) {
                InputManager.ActivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [OnEventComplete]
        public void CheckMouseState(NodeAddedEvent e, SelfBattleUser selfBattleUser, SingleNode<BattleActionsStateComponent> battleActionsState,
            Optional<SingleNode<SpectatorCameraComponent>> spectator) {
            if (spectator.IsPresent()) {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            } else if (selfBattleUser.mouseControlStateHolder.MouseControlAllowed && !InputManager.CheckAction(BattleActions.SHOW_SCORE)) {
                InputManager.ActivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        public class CheckMouseEvent : Event { }

        public class SelfBattleUser : Node {
            public MouseControlStateHolderComponent mouseControlStateHolder;

            public SelfBattleUserComponent selfBattleUser;
        }
    }
}