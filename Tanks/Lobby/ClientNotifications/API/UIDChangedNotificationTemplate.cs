using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1475750208936L)]
    public interface UIDChangedNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        UIDChangedNotificationTextComponent uIDChangedNotificationText();
    }
}