using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1453876318921L)]
    public class PaymentIsCancelledEvent : Event {
        public int ErrorCode { get; set; }
    }
}