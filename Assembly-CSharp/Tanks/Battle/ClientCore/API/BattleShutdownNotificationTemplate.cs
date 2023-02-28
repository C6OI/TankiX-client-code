using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1463040351057L)]
    public interface BattleShutdownNotificationTemplate : NotificationTemplate, Template {
        [PersistentConfig]
        [AutoAdded]
        BattleShutdownTextComponent battleShutdownText();

        [AutoAdded]
        UpdatedNotificationComponent updatedNotification();

        [AutoAdded]
        NotClickableNotificationComponent notClickableNotification();
    }
}