using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(636409752758272153L)]
    public class EnergyContextBuyEvent : Event {
        public EnergyContextBuyEvent() { }

        public EnergyContextBuyEvent(long count, long price) {
            Count = count;
            XPrice = price;
        }

        public long XPrice { get; set; }

        public long Count { get; set; }
    }
}