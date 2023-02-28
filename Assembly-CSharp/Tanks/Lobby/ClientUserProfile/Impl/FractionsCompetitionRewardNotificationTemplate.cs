using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1547017909507L)]
    public interface FractionsCompetitionRewardNotificationTemplate : LockScreenNotificationTemplate, NotificationTemplate, Template { }
}