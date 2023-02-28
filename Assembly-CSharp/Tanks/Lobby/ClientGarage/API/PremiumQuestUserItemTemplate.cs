using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513582138852L)]
    public interface PremiumQuestUserItemTemplate : GarageItemImagedTemplate, UserItemTemplate, DurationItemTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        PremiumQuestItemComponent premiumQuestItem();

        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();
    }
}