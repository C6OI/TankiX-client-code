using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Lobby.ClientUserProfile.API {
    [SerialVersionUID(1454656560829L)]
    public interface NotificationTemplate : Template {
        NotificationComponent notification();

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