using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [SerialVersionUID(1453891891716L)]
    [Shared]
    public class GoodsPriceComponent : Component {
        public string Currency { get; set; }

        public double Price { get; set; }
    }
}