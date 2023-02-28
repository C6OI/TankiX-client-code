using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1511430732717L)]
    public class ElevatedAccessUserChangeEnergyEvent : Event {
        public int Count { get; set; }
    }
}