using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.API {
    [SerialVersionUID(1473253631059L)]
    [Shared]
    public class GoodsXPriceComponent : Component {
        public long Price { get; set; }
    }
}