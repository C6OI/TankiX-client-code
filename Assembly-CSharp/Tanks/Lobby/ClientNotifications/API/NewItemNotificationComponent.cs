using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1481177510866L)]
    public class NewItemNotificationComponent : Component {
        public Entity Item { get; set; }

        public int Amount { get; set; }
    }
}