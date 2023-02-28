using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(636148933341269890L)]
    public interface IgnoreBattleScreenNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        IgnoreBattleScreenNotificationComponent ignoreBattleScreenNotification();
    }
}