using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636100801770520539L)]
    public interface GraffitiMarketItemTemplate : GraffitiItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}