using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(92197374614905239L)]
    public class RoundStopTimeComponent : Component {
        public Date StopTime { get; set; }
    }
}