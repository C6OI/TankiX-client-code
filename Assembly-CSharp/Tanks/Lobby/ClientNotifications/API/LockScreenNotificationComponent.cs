using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNotifications.API {
    public class LockScreenNotificationComponent : Component {
        public Entity ScreenEntity { get; set; }
    }
}