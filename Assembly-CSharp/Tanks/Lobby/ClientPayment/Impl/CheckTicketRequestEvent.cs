using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636303201540672590L)]
    [Shared]
    public class CheckTicketRequestEvent : Event {
        public CheckTicketRequestEvent(string steamId, string ticket) {
            SteamId = steamId;
            Ticket = ticket;
        }

        public string SteamId { get; set; }

        public string Ticket { get; set; }
    }
}