using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1482844606270L)]
    public class SubscribeChangeEvent : Event {
        public bool Subscribed { get; set; }
    }
}