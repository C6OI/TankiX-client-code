using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientFriends.API {
    public class FriendSentNotificationComponent : Component {
        public string Message { get; set; }
    }
}