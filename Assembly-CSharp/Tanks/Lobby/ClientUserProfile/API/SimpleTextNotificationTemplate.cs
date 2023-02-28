using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1493196797791L)]
    public interface SimpleTextNotificationTemplate : NotificationTemplate, Template {
        ServerNotificationMessageComponent serverNotificationMessage();
    }
}