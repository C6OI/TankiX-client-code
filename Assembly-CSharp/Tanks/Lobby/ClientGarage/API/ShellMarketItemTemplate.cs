using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(716181447780635764L)]
    public interface ShellMarketItemTemplate : ShellItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();

        [AutoAdded]
        [PersistentConfig]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        MountUserRankRestrictionComponent mountUserRankRestriction();
    }
}