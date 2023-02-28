using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [SerialVersionUID(1454505010104L)]
    [Shared]
    public class AdyenBuyGoodsByCardEvent : ProcessPaymentEvent {
        public string EncrypedCard { get; set; }
    }
}