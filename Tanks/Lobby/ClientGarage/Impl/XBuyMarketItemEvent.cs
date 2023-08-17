using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1473424321578L)]
    public class XBuyMarketItemEvent : Event {
        public int Price { get; set; }
    }
}