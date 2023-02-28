using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(636153272411746263L)]
    public interface LockScreenNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        LockScreenNotificationComponent lockScreenNotification();
    }
}