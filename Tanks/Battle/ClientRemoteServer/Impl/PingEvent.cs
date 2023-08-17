using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    [SerialVersionUID(5356229304896471086L)]
    [Shared]
    public class PingEvent : Event {
        public long ServerTime { get; set; }

        public sbyte CommandId { get; set; }
    }
}