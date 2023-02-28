using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1503551312217L)]
    public class CheckSteamDlcEvent : Event {
        public CheckSteamDlcEvent(string steamId, string ticket) {
            SteamId = steamId;
            Ticket = ticket;
        }

        public string SteamId { get; set; }

        public string Ticket { get; set; }
    }
}