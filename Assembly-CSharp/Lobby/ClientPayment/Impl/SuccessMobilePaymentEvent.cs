using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1465545777498L)]
    public class SuccessMobilePaymentEvent : Event {
        public string TransactionId { get; set; }
    }
}