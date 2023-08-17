using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636100801770520539L)]
    public interface GraffitiMarketItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate,
        GraffitiItemTemplate, ItemImagedTemplate, MarketItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();

        [PersistentConfig]
        [AutoAdded]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [PersistentConfig]
        [AutoAdded]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}