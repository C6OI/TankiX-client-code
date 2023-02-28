using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1459256177890L)]
    public class EmailNotConfirmedEvent : Event {
        public string Email { get; set; }
    }
}