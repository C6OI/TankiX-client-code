using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.API {
    public class NotificationMessageComponent : Component {
        public NotificationMessageComponent() { }

        public NotificationMessageComponent(string message) => Message = message;

        public string Message { get; set; }
    }
}