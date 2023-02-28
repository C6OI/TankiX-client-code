using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(2930474294118078222L)]
    public class IdleCounterComponent : Component {
        [ProtocolOptional] public Date? SkipBeginTime { get; set; }

        public long SkippedMillis { get; set; }
    }
}