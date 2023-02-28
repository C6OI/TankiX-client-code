using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(636147223268818488L)]
    public interface UserRankRewardNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, IgnoreBattleResultScreenNotificationTemplate,
        NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        UserRankRewardNotificationTextComponent userRankRewardNotificationText();
    }
}