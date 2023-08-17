using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientCommunicator.Impl {
    public class PrivateChatRequestComponent : Component {
        public PrivateChatRequestComponent(long userId, string username) {
            UserId = userId;
            Username = username;
        }

        public long UserId { get; set; }

        public string Username { get; set; }
    }
}