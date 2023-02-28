using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientProfile.API {
    [Shared]
    [SerialVersionUID(1513252416040L)]
    public class PremiumAccountBoostComponent : Component {
        public Date EndDate { get; set; }
    }
}