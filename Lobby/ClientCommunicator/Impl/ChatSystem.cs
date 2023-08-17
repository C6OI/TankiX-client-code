using Lobby.ClientCommunicator.API;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientCommunicator.Impl {
    public class ChatSystem : ECSSystem {
        [OnEventFire]
        public void AddChatToScreenGroup(UserEnterToChatEvent e, SelfUserNode userNode, SingleNode<ChatComponent> chatNode,
            [JoinAll] ActiveChatScreenNode screenNode) => screenNode.screenGroup.Attach(chatNode.Entity);

        [OnEventFire]
        public void ClearMessagesOnExit(NodeRemoveEvent e, SingleNode<ChatMessageGUIComponent> node) =>
            Object.Destroy(node.component.gameObject);

        [OnEventFire]
        public void DetachUserOnChatClose(NodeRemoveEvent e, ActiveChatScreenNode activeChatScreenNode,
            [JoinByScreen] SingleNode<ChatComponent> chatNode) => ScheduleEvent<UserDetachFromChatEvent>(chatNode.Entity);

        [OnEventFire]
        public void ActivateSendMessageButton(NodeAddedEvent e, ActiveChatScreenNode chat,
            [JoinByScreen] InputFieldNode messageField) {
            messageField.inputField.InputField.Select();
            messageField.inputField.InputField.ActivateInputField();
        }

        [OnEventFire]
        public void ShowMessageAuthorProfile(AvatarMessageButtonClickEvent e,
            SingleNode<ChatMessageAuthorComponent> chatMessageAuthor) => ScheduleEvent(
            new ShowProfileScreenEvent(chatMessageAuthor.component.Author.Id),
            chatMessageAuthor.component.Author);

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class ActiveChatScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ChatScreenComponent chatScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class InputFieldNode : Node {
            public ChatMessageInputComponent chatMessageInput;
            public InputFieldComponent inputField;
        }
    }
}