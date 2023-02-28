using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.API {
    [Shared]
    [SerialVersionUID(1471252962981L)]
    public class PaymentStatisticsEvent : Event {
        public PaymentStatisticsAction Action { get; set; }

        public long Item { get; set; }

        public long Method { get; set; }

        public string Screen { get; set; }
    }
}