using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(3963540336787160114L)]
    public class PingResultEvent : Event {
        public long ServerTime { get; set; }

        public float Ping { get; set; }
    }
}