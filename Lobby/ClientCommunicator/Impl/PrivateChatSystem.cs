using Lobby.ClientCommunicator.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientCommunicator.Impl {
    public class PrivateChatSystem : ECSSystem {
        [OnEventFire]
        public void DisableActiveUserList(ChatLoadedEvent e, SingleNode<PrivateChatComponent> chat,
            [JoinByScreen] ChatScreenNode screen) => screen.chatScreen.ActiveUserList.SetActive(false);

        [OnEventFire]
        public void SetHeader(NodeAddedEvent e, PrivateChatRequestOnScreenNode requestChatNode,
            [JoinByScreen] [Context] ChatScreenNode screenNode) {
            string username = requestChatNode.privateChatRequest.Username;
            screenNode.Entity.AddComponent(new ScreenHeaderTextComponent("Разговор с " + username));
        }

        public class ChatScreenNode : Node {
            public ChatScreenComponent chatScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class PrivateChatRequestOnScreenNode : Node {
            public PrivateChatRequestComponent privateChatRequest;

            public ScreenGroupComponent screenGroup;
        }
    }
}