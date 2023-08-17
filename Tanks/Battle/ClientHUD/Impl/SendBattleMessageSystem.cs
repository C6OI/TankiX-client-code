using Lobby.ClientCommunicator.API;
using Lobby.ClientCommunicator.Impl;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SendBattleMessageSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void SendMessageOnButtonClick(ButtonClickEvent e, SingleNode<SendMessageButtonComponent> sendMessageButton,
            [JoinByScreen] InputFieldNode inputFieldNode, [JoinByScreen] ActiveChannelNode activeChannelNode) =>
            SendMessage(activeChannelNode.Entity, inputFieldNode);

        [OnEventFire]
        public void SendMessageOnEnterPressed(UpdateEvent e, InputFieldNode inputFieldNode,
            [JoinByScreen] BattleChatStateNode battleChatStateNode, [JoinByScreen] ActiveChannelNode activeChannelNode) {
            if (InputManager.GetKeyDown(KeyCode.Return) || InputManager.GetKeyDown(KeyCode.KeypadEnter)) {
                SendMessage(activeChannelNode.Entity, inputFieldNode);
            }
        }

        void SendMessage(Entity chat, InputFieldNode inputFieldNode) {
            string text = ChatMessageUtil.RemoveWhiteSpaces(inputFieldNode.inputField.Input);

            text = ChatMessageUtil.RemoveTags(text,
                new string[2] {
                    RichTextTags.COLOR,
                    RichTextTags.SIZE
                });

            if (!string.IsNullOrEmpty(text)) {
                ScheduleEvent(new SendBattleChatMessageEvent(text), chat);
                ScheduleEvent<ClearBattleChatInputEvent>(chat);
            }
        }

        public class InputFieldNode : Node {
            public BattleChatMessageInputComponent battleChatMessageInput;
            public InputFieldComponent inputField;
        }

        public class ActiveChannelNode : Node {
            public ActiveChannelComponent activeChannel;
            public ChatComponent chat;
        }

        public class BattleChatStateNode : Node {
            public BattleChatStateComponent battleChatState;

            public ScreenGroupComponent screenGroup;
        }
    }
}