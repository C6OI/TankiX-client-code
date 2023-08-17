using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1459759123996L)]
    public interface ItemNotificationTemplate : Template {
        [AutoAdded]
        ItemNotificationComponent itemNotity();

        [PersistentConfig]
        [AutoAdded]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}