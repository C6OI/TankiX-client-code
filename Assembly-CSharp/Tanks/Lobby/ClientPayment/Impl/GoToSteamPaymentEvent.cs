using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636267643125901623L)]
    public class GoToSteamPaymentEvent : Event { }
}