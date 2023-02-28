using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientPayment.Impl {
    public class ProcessPaymentEvent : Event {
        public long TotalAmount { get; set; }
    }
}