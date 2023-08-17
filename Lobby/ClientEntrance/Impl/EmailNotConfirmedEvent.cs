using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(1459256177890L)]
    [Shared]
    public class EmailNotConfirmedEvent : Event {
        public string Email { get; set; }
    }
}