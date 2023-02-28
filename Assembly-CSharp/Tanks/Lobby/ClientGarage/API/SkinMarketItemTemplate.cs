using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1467632386864L)]
    public interface SkinMarketItemTemplate : SkinItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        MountUserRankRestrictionComponent mountUserRankRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}