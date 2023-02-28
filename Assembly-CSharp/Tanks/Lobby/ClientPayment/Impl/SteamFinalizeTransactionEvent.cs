using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636268972117317357L)]
    [Shared]
    public class SteamFinalizeTransactionEvent : Event {
        public SteamFinalizeTransactionEvent() { }

        public SteamFinalizeTransactionEvent(string orderId, long appId, bool autorized) {
            OrderId = orderId;
            AppId = appId;
            Autorized = autorized;
        }

        public string OrderId { get; set; }

        public long AppId { get; set; }

        public bool Autorized { get; set; }
    }
}