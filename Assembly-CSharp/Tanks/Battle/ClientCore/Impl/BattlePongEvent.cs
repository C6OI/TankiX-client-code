using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1480333153972L)]
    public class BattlePongEvent : Event {
        public float ClientSendRealTime { get; set; }
    }
}