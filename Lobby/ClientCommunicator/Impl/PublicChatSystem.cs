using Lobby.ClientCommunicator.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientCommunicator.Impl {
    public class PublicChatSystem : ECSSystem {
        [OnEventFire]
        public void EnableActiveUserList(ChatLoadedEvent e, SingleNode<PublicChatComponent> chat,
            [JoinByScreen] ChatScreenNode screen) => screen.chatScreen.ActiveUserList.SetActive(true);

        [OnEventFire]
        public void SetHeader(NodeAddedEvent e, PublicChatRequestOnScreenNode chatRequestNode,
            [JoinByScreen] [Context] ChatScreenNode screenNode) =>
            screenNode.Entity.AddComponent(new ScreenHeaderTextComponent(chatRequestNode.publicChatRequest.ChatName));

        public class PublicChatRequestOnScreenNode : Node {
            public PublicChatRequestComponent publicChatRequest;

            public ScreenGroupComponent screenGroup;
        }

        public class ChatScreenNode : Node {
            public ChatScreenComponent chatScreen;

            public ScreenGroupComponent screenGroup;
        }
    }
}