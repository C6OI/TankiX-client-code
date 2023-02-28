using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(636560579940505033L)]
    public class EventContainerNotificationComponent : Component {
        public long ContainerId { get; set; }
    }
}