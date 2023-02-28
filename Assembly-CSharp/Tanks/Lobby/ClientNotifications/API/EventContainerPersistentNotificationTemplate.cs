using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(636560576432081685L)]
    public interface EventContainerPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template { }
}