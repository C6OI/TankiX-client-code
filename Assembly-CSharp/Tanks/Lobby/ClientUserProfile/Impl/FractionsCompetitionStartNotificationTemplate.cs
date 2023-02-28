using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1544689375392L)]
    public interface FractionsCompetitionStartNotificationTemplate : LockScreenNotificationTemplate, NotificationTemplate, Template { }
}