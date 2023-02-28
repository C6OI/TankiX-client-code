using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1495542216706L)]
    public interface QuestRewardNotificationTemplate : LockScreenNotificationTemplate, IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        NewItemNotificationTextComponent newItemNotificationText();

        [AutoAdded]
        [PersistentConfig]
        NewPaintItemNotificationTextComponent newPaintItemNotificationText();
    }
}