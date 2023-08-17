using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(716181447780635764L)]
    public interface ShellMarketItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        MarketItemTemplate, ShellItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();

        [AutoAdded]
        [PersistentConfig]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();

        [PersistentConfig]
        [AutoAdded]
        MountUserRankRestrictionComponent mountUserRankRestriction();
    }
}