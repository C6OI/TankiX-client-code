using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.main.csharp.Impl.Platbox {
    [SerialVersionUID(1464785700114L)]
    [Shared]
    public class PlatBoxBuyGoodsEvent : Event {
        public string Phone { get; set; }
    }
}