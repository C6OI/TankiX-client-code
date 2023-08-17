using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientNotifications.Impl;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1459759228144L)]
    public interface NewItemNotificationTemplate : Template, ItemNotificationTemplate {
        [PersistentConfig]
        NewIemNotificationComponent newIemNotification();

        ItemNotificationUserItemComponent itemNotificationUserItem();
    }
}