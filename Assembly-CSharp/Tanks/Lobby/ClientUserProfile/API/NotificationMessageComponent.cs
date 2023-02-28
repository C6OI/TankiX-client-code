using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class NotificationMessageComponent : Component {
        public NotificationMessageComponent() { }

        public NotificationMessageComponent(string message) => Message = message;

        public string Message { get; set; }
    }
}