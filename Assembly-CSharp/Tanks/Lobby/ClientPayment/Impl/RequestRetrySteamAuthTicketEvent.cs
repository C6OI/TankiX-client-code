using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636281276780360995L)]
    public class RequestRetrySteamAuthTicketEvent : Event {
        public bool GoToPayment { get; set; }
    }
}