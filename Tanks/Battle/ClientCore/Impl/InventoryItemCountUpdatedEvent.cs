using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1437997432930L)]
    public class InventoryItemCountUpdatedEvent : Event {
        public long Count { get; set; }
    }
}