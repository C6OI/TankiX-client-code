using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1460029823788L)]
    public class ItemNotificationUserItemComponent : Component {
        public Entity Entity { get; set; }
    }
}