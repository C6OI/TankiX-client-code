using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class SendMessageSystem : ECSSystem {
        [OnEventFire]
        public void SetMessageLength(InputFieldValueChangedEvent e, InputFieldNode input, [JoinByScreen] ChatNode chat) {
            string input2 = input.inputField.Input;
            int maxMessageLength = chat.chatConfig.MaxMessageLength;

            if (input2.Length > maxMessageLength) {
                input.inputField.Input = input2.Remove(maxMessageLength);
            }
        }

        [OnEventFire]
        public void SendMessageOnButtonClick(ButtonClickEvent e, SingleNode<SendMessageButtonComponent> sendMessageButton, [JoinByScreen] InputFieldNode inputFieldNode,
            [JoinByScreen] ChatUINode chatUI, [JoinByScreen] ChatNode chat) {
            SendMessage(chat.Entity, chatUI, inputFieldNode);
        }

        [OnEventFire]
        public void SendMessageOnEnterPressed(UpdateEvent e, InputFieldNode inputFieldNode, [JoinByScreen] ChatUINode chatUI, [JoinByScreen] ChatNode chat) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                SendMessage(chat.Entity, chatUI, inputFieldNode);
            }
        }

        void SendMessage(Entity chat, ChatUINode chatUI, InputFieldNode inputFieldNode) {
            string text = ChatMessageUtil.RemoveWhiteSpaces(inputFieldNode.inputField.Input);

            text = ChatMessageUtil.RemoveTags(text, new string[2] {
                RichTextTags.COLOR,
                RichTextTags.SIZE
            });

            if (!string.IsNullOrEmpty(text)) {
                ScheduleEvent(new SendChatMessageEvent(text), chat);
                inputFieldNode.inputField.Input = string.Empty;
                inputFieldNode.inputField.InputField.Select();
                inputFieldNode.inputField.InputField.ActivateInputField();
                chatUI.chatUI.SavedInputMessage = string.Empty;
            }
        }

        [Mandatory]
        [OnEventFire]
        public void SendLobbyMessage(SendMessageEvent e, Node any, [JoinAll] ActiveLobbyChat activeChannel, [JoinAll] SingleNode<SelfUserComponent> self) {
            Event commandEvent;

            if (ChatCommands.IsCommand(e.Message, out commandEvent)) {
                NewEvent(commandEvent).Attach(activeChannel).Attach(self).Schedule();
            } else {
                ScheduleEvent(new SendChatMessageEvent(e.Message), activeChannel);
            }
        }

        [OnEventFire]
        public void OpenChat(ChatMessageClickEvent e, Node any, [JoinAll] ActiveLobbyChat activeChannel, [JoinAll] SingleNode<SelfUserComponent> self) {
            NewEvent(new OpenPersonalChannelEvent {
                UserUid = e.Link
            }).Attach(self).Attach(activeChannel).Schedule();
        }

        [OnEventFire]
        public void OpenChat(OpenPersonalChatFromContextMenuEvent e, SingleNode<UserUidComponent> friend, [JoinAll] SingleNode<SelfUserComponent> self,
            [JoinAll] ActiveLobbyChat activeChannel, [JoinAll] SingleNode<ChatDialogComponent> dialog) {
            if (!dialog.component.IsOpen() && !dialog.component.IsHidden()) {
                dialog.component.Maximaze();
            }

            NewEvent(new OpenPersonalChannelEvent {
                UserUid = friend.component.Uid.Replace("Deserter ", string.Empty)
            }).Attach(self).Attach(activeChannel).Schedule();
        }

        public class InputFieldNode : Node {
            public ChatMessageInputComponent chatMessageInput;
            public InputFieldComponent inputField;
        }

        public class ChatNode : Node {
            public ActiveBattleChannelComponent activeBattleChannel;
            public ChatComponent chat;

            public ChatConfigComponent chatConfig;
        }

        public class ChatUINode : Node {
            public ChatUIComponent chatUI;
        }

        public class ActiveLobbyChat : Node {
            public ActiveChannelComponent activeChannel;

            public ChatChannelComponent chatChannel;
        }
    }
}