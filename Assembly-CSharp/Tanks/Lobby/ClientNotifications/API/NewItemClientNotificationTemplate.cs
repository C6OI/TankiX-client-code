using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1486963714687L)]
    public interface NewItemClientNotificationTemplate : NewItemNotificationTemplate, LockScreenNotificationTemplate, IgnoreBattleScreenNotificationTemplate, NotificationTemplate,
        Template {
        [AutoAdded]
        NewItemClientNotificationComponent newItemClientNotification();
    }
}