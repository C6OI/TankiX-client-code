using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1462962043959L)]
    public interface ServerShutdownNotificationTemplate : NotificationTemplate, Template {
        [PersistentConfig]
        [AutoAdded]
        ServerShutdownTextComponent serverShutdownText();

        [AutoAdded]
        UpdatedNotificationComponent updatedNotification();

        [AutoAdded]
        NotClickableNotificationComponent notClickableNotification();
    }
}