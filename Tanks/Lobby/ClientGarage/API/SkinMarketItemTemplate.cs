using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1467632386864L)]
    public interface SkinMarketItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        MarketItemTemplate, SkinItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        MountUserRankRestrictionComponent mountUserRankRestriction();

        [PersistentConfig]
        [AutoAdded]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [PersistentConfig]
        [AutoAdded]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}