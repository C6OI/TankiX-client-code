using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.main.csharp.Impl.Platbox {
    [Shared]
    [SerialVersionUID(1464785700114L)]
    public class PlatBoxBuyGoodsEvent : ProcessPaymentEvent {
        public string Phone { get; set; }
    }
}