using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [SerialVersionUID(1455278054547L)]
    [Shared]
    public class GameCurrencyPackComponent : Component {
        public int Amount { get; set; }

        public int Bonus { get; set; }
    }
}