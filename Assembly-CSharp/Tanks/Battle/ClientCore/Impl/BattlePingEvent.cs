using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1480326022618L)]
    public class BattlePingEvent : Event {
        public float ClientSendRealTime { get; set; }
    }
}