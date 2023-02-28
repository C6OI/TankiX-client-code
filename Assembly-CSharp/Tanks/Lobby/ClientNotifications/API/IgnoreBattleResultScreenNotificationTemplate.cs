using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(636153325057457425L)]
    public interface IgnoreBattleResultScreenNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification();
    }
}