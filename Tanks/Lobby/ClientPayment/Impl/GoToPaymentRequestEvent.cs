using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(1470652819513L)]
    [Shared]
    public class GoToPaymentRequestEvent : Event { }
}