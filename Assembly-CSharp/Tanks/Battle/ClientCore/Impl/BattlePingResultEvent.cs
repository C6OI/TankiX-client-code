using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1480333679186L)]
    public class BattlePingResultEvent : Event {
        public float ClientSendRealTime { get; set; }

        public float ClientReceiveRealTime { get; set; }
    }
}