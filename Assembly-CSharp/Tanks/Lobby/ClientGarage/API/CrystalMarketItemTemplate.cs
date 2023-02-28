using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1479898113503L)]
    public interface CrystalMarketItemTemplate : CrystalItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        CardImageItemComponent cardImageItem();
    }
}