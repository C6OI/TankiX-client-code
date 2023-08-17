using Lobby.ClientCommunicator.Impl;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleHUDStateControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void SetBattleChatStateOnEnterPressed(UpdateEvent e, NotSpectatorBattleChatGUINode battleChatGUINode,
            [JoinByScreen] HUDScreenNode hudScreenNode) {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_CHAT)) {
                SetBattleChatState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventComplete]
        public void SetBattleActionsStateOnEscPressed(UpdateEvent e, NotSpectatorBattleChatGUINode battleChatGUINode,
            [JoinByScreen] HUDScreenNode hudScreenNode) {
            if (InputManager.GetActionKeyDown(BattleChatActions.CLOSE_CHAT)) {
                SetBattleActionsState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventComplete]
        public void SetBattleActionsStateOnEnterPressed(UpdateEvent e, SingleNode<BattleChatStateComponent> battleChatState,
            [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode) {
            if (InputManager.GetKeyDown(KeyCode.Return) || InputManager.GetKeyDown(KeyCode.KeypadEnter)) {
                SetBattleActionsState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventFire]
        public void SetBattleActionsStateOnSendMessageButtonClick(ButtonClickEvent e,
            SingleNode<SendMessageButtonComponent> button, [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode,
            [JoinByScreen] HUDScreenNode hudScreenNode) => SetBattleActionsState(battleChatGUINode, hudScreenNode);

        [OnEventFire]
        public void SetBattleActionsStateOnExit(GoBackFromBattleEvent e, Node any,
            [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode) =>
            SetBattleActionsState(battleChatGUINode, hudScreenNode);

        [OnEventFire]
        public void GoBackClick(ButtonClickEvent e, SingleNode<BattleBackButtonComponent> battleBackButton) =>
            ScheduleEvent<RequestGoBackFromBattleEvent>(battleBackButton.Entity);

        [OnEventFire]
        public void GoBack(GoBackFromBattleEvent e, Node any, [JoinAll] BattleScreenNode battleScreen,
            [JoinByBattle] SingleNode<BattleComponent> battle) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<BattleSelectLoadScreenComponent>>(battle);

        [OnEventFire]
        public void ActivateInputField(NodeAddedEvent e, InputFieldNode inputFieldNode,
            [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode) {
            inputFieldNode.inputField.Input = battleChatGUINode.battleChatGUI.SavedInputMessage.Trim();
            inputFieldNode.inputField.InputField.Select();
            inputFieldNode.inputField.InputField.ActivateInputField();
        }

        [OnEventFire]
        public void SetBattleActionsStateOnEnter(NodeAddedEvent e, NotSpectatorBattleChatGUINode battleChatGUINode,
            HUDScreenNode hudScreenNode) => SetBattleActionsState(battleChatGUINode, hudScreenNode);

        void SetBattleActionsState(NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode) {
            battleChatGUINode.battleChatGUI.InputPanelActivity = false;
            hudScreenNode.battleHUDESM.Esm.ChangeState<BattleHUDStates.ActionsState>();
        }

        void SetBattleChatState(NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode) {
            battleChatGUINode.battleChatGUI.InputPanelActivity = true;
            hudScreenNode.battleHUDESM.Esm.ChangeState<BattleHUDStates.ChatState>();
        }

        [OnEventFire]
        public void ClearChatOnExit(GoBackFromBattleEvent e, Node any, [JoinAll] BattleChatGUINode battleChatGUINode) {
            BattleChatGUIComponent battleChatGUI = battleChatGUINode.battleChatGUI;
            battleChatGUI.InputHintText = string.Empty;
            battleChatGUI.InputFieldColor = battleChatGUI.CommonTextColor;
            battleChatGUINode.battleChatGUI.SavedInputMessage = string.Empty;

            foreach (Transform item in battleChatGUI.MessagesContainer.transform) {
                if (item.gameObject.GetComponent<BattleChatMessageGUIComponent>() != null) {
                    Object.Destroy(item.gameObject);
                }
            }
        }

        public class HUDScreenNode : Node {
            public BattleHUDESMComponent battleHUDESM;
            public BattleScreenComponent battleScreen;

            public ScreenGroupComponent screenGroup;
        }

        [Not(typeof(BattleChatSpectatorComponent))]
        public class NotSpectatorBattleChatGUINode : Node {
            public BattleChatGUIComponent battleChatGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleChatGUINode : Node {
            public BattleChatGUIComponent battleChatGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class InputFieldNode : Node {
            public BattleChatMessageInputComponent battleChatMessageInput;
            public InputFieldComponent inputField;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleScreenNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleScreenComponent battleScreen;
        }
    }
}