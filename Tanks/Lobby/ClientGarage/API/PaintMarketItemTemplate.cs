using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1436443339132L)]
    public interface PaintMarketItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        MarketItemTemplate, PaintItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();

        [PersistentConfig]
        [AutoAdded]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}