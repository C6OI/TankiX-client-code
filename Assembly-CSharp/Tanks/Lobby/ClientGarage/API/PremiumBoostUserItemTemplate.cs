using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513580884352L)]
    public interface PremiumBoostUserItemTemplate : GarageItemImagedTemplate, UserItemTemplate, DurationItemTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        PremiumBoostItemComponent premiumBoostItem();

        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();
    }
}