using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1522660970570L)]
    public class ElevatedAccessUserChangeReputationEvent : Event {
        public int Count { get; set; }
    }
}