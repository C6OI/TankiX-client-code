using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1436434151462L)]
    [Shared]
    public class ItemUpgradeUpdatedEvent : Event {
        public int Level { get; set; }
    }
}