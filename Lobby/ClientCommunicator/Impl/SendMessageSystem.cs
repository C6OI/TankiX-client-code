using Lobby.ClientCommunicator.API;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientCommunicator.Impl {
    public class SendMessageSystem : ECSSystem {
        [OnEventFire]
        public void SetMessageLength(InputFieldValueChangedEvent e, InputFieldNode input,
            [JoinByScreen] SingleNode<ChatConfigComponent> chatConfig) {
            string input2 = input.inputField.Input;
            int maxMessageLength = chatConfig.component.MaxMessageLength;

            if (input2.Length > maxMessageLength) {
                input.inputField.Input = input2.Remove(maxMessageLength);
            }
        }

        [OnEventFire]
        public void SendMessageOnButtonClick(ButtonClickEvent e, SingleNode<SendMessageButtonComponent> button,
            [JoinByScreen] InputFieldNode messageField, [JoinByScreen] SingleNode<ChatComponent> chatNode) =>
            SendMessage(chatNode.Entity, messageField);

        [OnEventFire]
        public void SendMessageOnEnterPressed(UpdateEvent e, InputFieldNode messageField,
            [JoinByScreen] SingleNode<ChatComponent> chatNode) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                SendMessage(chatNode.Entity, messageField);
            }
        }

        [OnEventFire]
        public void ClearInput(NodeAddedEvent e, InputFieldNode messageField) =>
            messageField.inputField.Input = string.Empty;

        void SendMessage(Entity chat, InputFieldNode messageField) {
            string input = messageField.inputField.Input;

            if (!string.IsNullOrEmpty(input)) {
                ScheduleEvent(new SendChatMessageEvent(input), chat);
                messageField.inputField.Input = string.Empty;
                messageField.inputField.InputField.Select();
                messageField.inputField.InputField.ActivateInputField();
            }
        }

        public class InputFieldNode : Node {
            public ChatMessageInputComponent chatMessageInput;
            public InputFieldComponent inputField;
        }
    }
}