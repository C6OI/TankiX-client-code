using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1493022950509L)]
    public class UserBlockedEvent : Event {
        public string Reason { get; set; }
    }
}