using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1480931079801L)]
    public class ItemsCountChangedEvent : Event {
        public long Delta { get; set; }
    }
}