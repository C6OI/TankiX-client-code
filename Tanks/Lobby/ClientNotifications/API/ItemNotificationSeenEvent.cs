using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1459839444539L)]
    public class ItemNotificationSeenEvent : Event { }
}