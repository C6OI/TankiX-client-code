using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1514367057340L)]
    public class ElevatedAccessUserCreateItemEvent : Event {
        public long Count { get; set; }

        public long ItemId { get; set; }
    }
}