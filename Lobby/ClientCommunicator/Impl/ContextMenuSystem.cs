using Lobby.ClientCommunicator.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientCommunicator.Impl {
    public class ContextMenuSystem : ECSSystem {
        [OnEventFire]
        public void ShowContextMenuOnMessageClick(ChatMessageButtonClickEvent e, MessageNode message,
            [JoinAll] SingleNode<ChatScreenComponent> chatScreen, [JoinByScreen] SingleNode<PublicChatComponent> publicChat,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            if (!selfUser.Entity.Equals(message.chatMessageAuthor.Author)) {
                chatScreen.component.FavoritesAndUsers.gameObject.SetActive(false);
                chatScreen.component.MessageContextMenu.SetActive(true);

                chatScreen.component.MessageContextMenu.GetComponent<MessageContextMenuComponent>().SelectedMessage =
                    message.Entity;
            }
        }

        [OnEventFire]
        public void HideContextMenuOnCloseButtonClick(ButtonClickEvent e, SingleNode<CloseMenuButtonComponent> button,
            [JoinAll] SingleNode<ChatScreenComponent> chatScreen) {
            chatScreen.component.MessageContextMenu.SetActive(false);
            chatScreen.component.MessageContextMenu.GetComponent<MessageContextMenuComponent>().SelectedMessage = null;
            chatScreen.component.FavoritesAndUsers.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void OpenPrivateChatOnButtonClick(ButtonClickEvent e, SingleNode<OpenPrivateChatButtonComponent> button,
            [JoinAll] SingleNode<ChatScreenComponent> chatScreen) {
            Entity selectedMessage = chatScreen.component.MessageContextMenu.GetComponent<MessageContextMenuComponent>()
                .SelectedMessage;

            ScheduleEvent<OpenPrivateChatByMessageEvent>(selectedMessage);
            chatScreen.component.MessageContextMenu.gameObject.SetActive(false);
            chatScreen.component.FavoritesAndUsers.gameObject.SetActive(true);
        }

        [Mandatory]
        [OnEventFire]
        public void EnterPrivateChat(OpenPrivateChatByMessageEvent e, MessageNode message) =>
            ScheduleEvent<CreatePrivateChatRequestEvent>(message.chatMessageAuthor.Author);

        public class MessageNode : Node {
            public ChatMessageAuthorComponent chatMessageAuthor;
            public ChatMessageGUIComponent chatMessageGui;
        }
    }
}