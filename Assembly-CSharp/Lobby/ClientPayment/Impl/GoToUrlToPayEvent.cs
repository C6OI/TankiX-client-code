using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1453876266331L)]
    public class GoToUrlToPayEvent : Event {
        public string Url { get; set; }
    }
}