using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1454656560829L)]
    public interface NotificationTemplate : Template {
        NotificationComponent notification();

        NotificationInstanceComponent notificationInstance();

        [AutoAdded]
        [PersistentConfig]
        NotificationConfigComponent notificationConfig();

        [AutoAdded]
        [PersistentConfig]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}