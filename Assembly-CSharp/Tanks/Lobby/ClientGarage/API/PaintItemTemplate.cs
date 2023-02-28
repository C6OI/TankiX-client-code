using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636287224884828984L)]
    public interface PaintItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        PaintItemComponent paintItem();

        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        MountableItemComponent mountableItem();
    }
}