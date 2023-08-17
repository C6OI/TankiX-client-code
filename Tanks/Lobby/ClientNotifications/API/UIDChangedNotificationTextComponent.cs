using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNotifications.API {
    public class UIDChangedNotificationTextComponent : Component {
        public string MessageTemplate { get; set; }
    }
}