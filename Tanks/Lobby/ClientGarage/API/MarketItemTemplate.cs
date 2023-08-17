using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433762481661L)]
    public interface MarketItemTemplate : Template {
        [AutoAdded]
        MarketItemComponent marketItem();

        [AutoAdded]
        [PersistentConfig("", true)]
        PriceItemComponent priceItem();

        [PersistentConfig("", true)]
        [AutoAdded]
        XPriceItemComponent xPriceItem();
    }
}