using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientCommunicator.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleHUDStateControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void SetBattleChatStateOnEnterPressed(UpdateEvent e, NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode) {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_CHAT)) {
                SetBattleChatState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventComplete]
        public void SetBattleActionsStateOnEscPressed(UpdateEvent e, NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode) {
            if (InputManager.GetActionKeyDown(BattleChatActions.CLOSE_CHAT)) {
                SetBattleActionsState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventComplete]
        public void SetBattleActionsStateOnEnterPressed(UpdateEvent e, SingleNode<BattleChatStateComponent> battleChatState, [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode,
            [JoinByScreen] HUDScreenNode hudScreenNode) {
            if (InputManager.GetKeyDown(KeyCode.Return) || InputManager.GetKeyDown(KeyCode.KeypadEnter)) {
                SetBattleActionsState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventFire]
        public void SetBattleActionsStateOnSendMessageButtonClick(ButtonClickEvent e, SingleNode<SendMessageButtonComponent> button,
            [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode) {
            SetBattleActionsState(battleChatGUINode, hudScreenNode);
        }

        [OnEventFire]
        public void SetBattleActionsStateOnExit(GoBackFromBattleEvent e, Node any, [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode) {
            SetBattleActionsState(battleChatGUINode, hudScreenNode);
        }

        [OnEventFire]
        public void SetBattleActionsStateOnEnter(NodeAddedEvent e, NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode) {
            SetBattleActionsState(battleChatGUINode, hudScreenNode);
        }

        void SetBattleActionsState(NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode) {
            battleChatGUINode.chatUI.InputPanelActivity = false;
            hudScreenNode.battleHUDESM.Esm.ChangeState<BattleHUDStates.ActionsState>();
        }

        void SetBattleChatState(NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode) {
            battleChatGUINode.chatUI.InputPanelActivity = true;
            hudScreenNode.battleHUDESM.Esm.ChangeState<BattleHUDStates.ChatState>();
        }

        public class HUDScreenNode : Node {
            public BattleHUDESMComponent battleHUDESM;
            public BattleScreenComponent battleScreen;

            public ScreenGroupComponent screenGroup;
        }

        [Not(typeof(BattleChatSpectatorComponent))]
        public class NotSpectatorBattleChatGUINode : Node {
            public BattleChatUIComponent battleChatUI;

            public ChatUIComponent chatUI;

            public ScreenGroupComponent screenGroup;
        }
    }
}