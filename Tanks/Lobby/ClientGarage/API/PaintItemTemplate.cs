using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1436443208731L)]
    public interface PaintItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate {
        [AutoAdded]
        PaintItemComponent colorItem();

        [PersistentConfig("unityAsset")]
        [AutoAdded]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        MountableItemComponent mountableItem();
    }
}