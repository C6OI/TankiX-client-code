using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636390988457169067L)]
    public interface SlotMarketItemTemplate : Template {
        [AutoAdded]
        MarketItemComponent marketItem();

        [AutoAdded]
        SlotItemComponent slotItem();
    }
}