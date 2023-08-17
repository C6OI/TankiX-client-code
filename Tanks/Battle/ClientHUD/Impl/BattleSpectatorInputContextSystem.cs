using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleSpectatorInputContextSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void ActivateBattleChatContext(PointEnterToBattleChatScrollViewEvent e,
            BattleChatSpectatorNotPressedOutOfFocusNode battleChatSpectator) {
            InputManager.ActivateContext(BattleChatContexts.BATTLE_CHAT);
            battleChatSpectator.Entity.AddComponent<BattleChatInFocusComponent>();
        }

        [OnEventFire]
        public void SetChatInFocus(PointEnterToBattleChatScrollViewEvent e,
            BattleChatSpectatorPressedOutOfFocusNode battleChatSpectator) =>
            battleChatSpectator.Entity.AddComponent<BattleChatInFocusComponent>();

        [OnEventFire]
        public void SetChatOutOfFocus(PointExitFromBattleChatScrollViewEvent e,
            BattleChatSpectatorPressedInFocusNode battleChatSpectator) =>
            battleChatSpectator.Entity.RemoveComponent<BattleChatInFocusComponent>();

        [OnEventFire]
        public void DeactivateBattleChatContext(PointExitFromBattleChatScrollViewEvent e,
            BattleChatSpectatorNotPressedInFocusNode battleChatSpectator) {
            InputManager.DeactivateContext(BattleChatContexts.BATTLE_CHAT);
            battleChatSpectator.Entity.RemoveComponent<BattleChatInFocusComponent>();
        }

        [OnEventFire]
        public void MouseButtonDownInChat(UpdateEvent evt, BattleChatSpectatorNotPressedInFocusNode battleChatSpectator) {
            if (InputManager.GetMouseButtonDown(UnityInputConstants.MOUSE_BUTTON_LEFT)) {
                battleChatSpectator.Entity.AddComponent<BattleChatStartDraggingComponent>();
            }
        }

        [OnEventFire]
        public void MouseButtonUpOutOfChat(UpdateEvent evt, BattleChatSpectatorPressedOutOfFocusNode battleChatSpectator) {
            if (InputManager.GetMouseButtonUp(UnityInputConstants.MOUSE_BUTTON_LEFT)) {
                InputManager.DeactivateContext(BattleChatContexts.BATTLE_CHAT);
                battleChatSpectator.Entity.RemoveComponent<BattleChatStartDraggingComponent>();
            }
        }

        [OnEventFire]
        public void MouseButtonUpInChat(UpdateEvent evt, BattleChatSpectatorPressedInFocusNode battleChatSpectator) {
            if (InputManager.GetMouseButtonUp(UnityInputConstants.MOUSE_BUTTON_LEFT)) {
                battleChatSpectator.Entity.RemoveComponent<BattleChatStartDraggingComponent>();
            }
        }

        public class BattleChatSpectatorNode : Node {
            public BattleChatSpectatorComponent battleChatSpectator;
        }

        public class BattleChatSpectatorPressedInFocusNode : BattleChatSpectatorNode {
            public BattleChatInFocusComponent battleChatInFocus;

            public BattleChatStartDraggingComponent battleChatStartDragging;
        }

        [Not(typeof(BattleChatStartDraggingComponent))]
        public class BattleChatSpectatorNotPressedInFocusNode : BattleChatSpectatorNode {
            public BattleChatInFocusComponent battleChatInFocus;
        }

        [Not(typeof(BattleChatInFocusComponent))]
        public class BattleChatSpectatorPressedOutOfFocusNode : BattleChatSpectatorNode {
            public BattleChatStartDraggingComponent battleChatStartDragging;
        }

        [Not(typeof(BattleChatStartDraggingComponent))]
        [Not(typeof(BattleChatInFocusComponent))]
        public class BattleChatSpectatorNotPressedOutOfFocusNode : BattleChatSpectatorNode { }
    }
}