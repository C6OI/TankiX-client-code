using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(5356229304896471086L)]
    public class PingEvent : Event {
        public long ServerTime { get; set; }

        public sbyte CommandId { get; set; }
    }
}