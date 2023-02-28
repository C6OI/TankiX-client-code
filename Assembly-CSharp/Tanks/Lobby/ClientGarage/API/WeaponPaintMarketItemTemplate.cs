using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636287153836461132L)]
    public interface WeaponPaintMarketItemTemplate : WeaponPaintItemTemplate, MarketItemTemplate, PaintItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        Template {
        [AutoAdded]
        [PersistentConfig]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [AutoAdded]
        [PersistentConfig]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}