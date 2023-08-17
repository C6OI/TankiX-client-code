using Lobby.ClientUserProfile.API;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1462962043959L)]
    public interface ServerShutdownNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ServerShutdownTextComponent serverShutdownText();

        [AutoAdded]
        UpdatedNotificationComponent updatedNotification();

        [AutoAdded]
        NotClickableNotificationComponent notClickableNotification();
    }
}