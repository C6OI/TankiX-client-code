using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1115422024552825915L)]
    public class PongEvent : Event {
        public float PongCommandClientRealTime { get; set; }

        public sbyte CommandId { get; set; }
    }
}