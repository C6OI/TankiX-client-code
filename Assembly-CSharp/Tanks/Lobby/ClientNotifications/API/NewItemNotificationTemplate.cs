using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1481176055388L)]
    public interface NewItemNotificationTemplate : LockScreenNotificationTemplate, IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        NewItemNotificationTextComponent newItemNotificationText();

        [AutoAdded]
        [PersistentConfig]
        NewPaintItemNotificationTextComponent newPaintItemNotificationText();
    }
}