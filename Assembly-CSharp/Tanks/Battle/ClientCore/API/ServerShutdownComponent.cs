using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1463118148654L)]
    [Shared]
    public class ServerShutdownComponent : Component {
        public Date StartDate { get; set; }

        public Date StopDateForClient { get; set; }
    }
}