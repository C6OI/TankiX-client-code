using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1489474099632L)]
    public interface ContainerPackPriceMarketItemTemplate : ContainerMarketItemTemplate, ContainerItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate,
        ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        PackPriceComponent packPrice();
    }
}