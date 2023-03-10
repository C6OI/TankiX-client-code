using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(5192591761194414739L)]
    public class DurationComponent : Component {
        public Date StartedTime { get; set; }
    }
}