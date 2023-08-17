using Lobby.ClientCommunicator.API;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatInputSystem : ECSSystem {
        [OnEventFire]
        public void SaveInputMessage(InputFieldValueChangedEvent e, InputFieldNode inputFieldNode,
            [JoinAll] BattleChatGUINode battleChatGUINode) {
            if (inputFieldNode.inputField.InputField.isFocused) {
                battleChatGUINode.battleChatGUI.SavedInputMessage = inputFieldNode.inputField.Input;
            }
        }

        [OnEventFire]
        public void ClearSavedInputMessage(ClearBattleChatInputEvent e, Node anyNode,
            [JoinAll] BattleChatGUINode battleChatGUINode) =>
            battleChatGUINode.battleChatGUI.SavedInputMessage = string.Empty;

        [OnEventFire]
        public void SetMaxMessageLength(NodeAddedEvent e, InputFieldNode inputFieldNode, [Combine] ChatNode chatNode,
            [JoinByChat] SingleNode<ChatConfigComponent> chatConfig) =>
            inputFieldNode.inputField.InputField.characterLimit = chatConfig.component.MaxMessageLength;

        public class InputFieldNode : Node {
            public BattleChatMessageInputComponent battleChatMessageInput;
            public InputFieldComponent inputField;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleChatGUINode : Node {
            public BattleChatGUIComponent battleChatGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class ChatNode : Node {
            public ChatComponent chat;

            public ChatGroupComponent chatGroup;

            public ScreenGroupComponent screenGroup;
        }
    }
}