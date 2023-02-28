using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatSystem : ECSSystem {
        [OnEventFire]
        public void ActivateInputField(NodeAddedEvent e, InputFieldNode inputFieldNode, [JoinByScreen] ChatUINode chatUI) {
            inputFieldNode.inputField.Input = string.Empty;
            inputFieldNode.inputField.InputField.Select();
            inputFieldNode.inputField.InputField.ActivateInputField();
            chatUI.chatUI.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void CheckoInputFieldFocus(UpdateEvent e, BattleInputFieldNode inputFieldNode) {
            if (!inputFieldNode.inputField.InputField.isFocused) {
                inputFieldNode.inputField.InputField.Select();
                inputFieldNode.inputField.InputField.ActivateInputField();
            }
        }

        [OnEventFire]
        public void ClearChatOnExit(NodeRemoveEvent e, ChatUINode chatUI) {
            ChatUIComponent chatUI2 = chatUI.chatUI;
            chatUI2.InputHintText = string.Empty;
            Color commonTextColor = chatUI2.CommonTextColor;
            chatUI2.BottomLineColor = commonTextColor;
            chatUI2.SavedInputMessage = string.Empty;
            chatUI2.MessagesContainer.GetComponent<ChatContentGUIComponent>().ClearMessages();
        }

        public class ChatUINode : Node {
            public ChatUIComponent chatUI;
        }

        public class InputFieldNode : Node {
            public ChatMessageInputComponent chatMessageInput;
            public InputFieldComponent inputField;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleInputFieldNode : InputFieldNode {
            public BattleChatMessageInputComponent battleChatMessageInput;
        }
    }
}