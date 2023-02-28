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

        [AutoAdded]
        [PersistentConfig("", true)]
        XPriceItemComponent xPriceItem();

        [AutoAdded]
        [PersistentConfig("", true)]
        ItemRarityComponent itemRarity();
    }
}