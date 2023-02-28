using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1470652819513L)]
    public class GoToPaymentRequestEvent : Event {
        string steamId;

        string ticket;

        public bool SteamIsActive { get; set; }

        public string SteamId {
            get => steamId ?? string.Empty;
            set => steamId = value;
        }

        public string Ticket {
            get => ticket ?? string.Empty;
            set => ticket = value;
        }
    }
}