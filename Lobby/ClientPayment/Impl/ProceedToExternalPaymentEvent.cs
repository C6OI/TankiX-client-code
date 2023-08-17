using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1453876160804L)]
    public class ProceedToExternalPaymentEvent : Event { }
}