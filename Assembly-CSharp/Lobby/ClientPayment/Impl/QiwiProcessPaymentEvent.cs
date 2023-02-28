using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1480668800792L)]
    public class QiwiProcessPaymentEvent : ProcessPaymentEvent {
        public string Account { get; set; }
    }
}