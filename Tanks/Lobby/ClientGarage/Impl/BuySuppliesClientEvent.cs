using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1458203368233L)]
    public class BuySuppliesClientEvent : Event {
        public int Count { get; set; }

        public long TotalPrice { get; set; }
    }
}