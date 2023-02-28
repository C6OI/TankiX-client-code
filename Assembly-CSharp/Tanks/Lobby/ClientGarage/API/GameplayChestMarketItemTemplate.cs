using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1487149202122L)]
    public interface GameplayChestMarketItemTemplate : ContainerItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        GameplayChestItemComponent gameplayChestItem();

        [AutoAdded]
        [PersistentConfig]
        TargetTierComponent targetTier();
    }
}