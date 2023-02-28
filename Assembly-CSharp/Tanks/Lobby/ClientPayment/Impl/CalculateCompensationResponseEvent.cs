using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1489085883534L)]
    public class CalculateCompensationResponseEvent : Event {
        public long Amount { get; set; }
    }
}