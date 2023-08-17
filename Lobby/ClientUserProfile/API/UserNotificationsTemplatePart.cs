using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.src.main.csharp.Impl.Notifications.Component;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.API {
    [TemplatePart]
    public interface UserNotificationsTemplatePart : UserTemplate, Template {
        [AutoAdded]
        NotificationsGroupComponent notificationsGroup();
    }
}