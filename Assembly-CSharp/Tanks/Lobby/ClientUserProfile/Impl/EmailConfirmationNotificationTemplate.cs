using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(636147145597704669L)]
    public interface EmailConfirmationNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        EmailConfirmationNotificationComponent emailConfirmationNotification();
    }
}