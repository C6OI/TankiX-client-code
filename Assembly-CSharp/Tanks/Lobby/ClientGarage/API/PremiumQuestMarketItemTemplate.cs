using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513580238036L)]
    public interface PremiumQuestMarketItemTemplate : GarageItemImagedTemplate, MarketItemTemplate, DurationItemTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        PremiumQuestItemComponent premiumQuestItem();

        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        [PersistentConfig]
        CardImageItemComponent cardImageItem();
    }
}