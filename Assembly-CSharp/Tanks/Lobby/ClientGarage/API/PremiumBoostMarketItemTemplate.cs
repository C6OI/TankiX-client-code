using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513580195801L)]
    public interface PremiumBoostMarketItemTemplate : GarageItemImagedTemplate, MarketItemTemplate, DurationItemTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        PremiumBoostItemComponent premiumBoostItem();

        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        [PersistentConfig]
        CardImageItemComponent cardImageItem();
    }
}