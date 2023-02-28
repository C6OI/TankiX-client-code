using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1480325268669L)]
    public class OpenContainerEvent : Event {
        public long Amount { get; set; } = 1L;
    }
}