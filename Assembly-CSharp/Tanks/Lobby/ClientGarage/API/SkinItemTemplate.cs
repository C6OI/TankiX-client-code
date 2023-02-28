using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1468229622177L)]
    public interface SkinItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        SkinItemComponent skinItem();

        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        MountableItemComponent mountableItem();
    }
}