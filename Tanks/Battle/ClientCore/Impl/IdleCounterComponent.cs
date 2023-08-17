using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(2930474294118078222L)]
    public class IdleCounterComponent : Component {
        public Optional<Date> SkipBeginTime { get; set; }

        public long SkippedMillis { get; set; }
    }
}