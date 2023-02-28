using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientProfile.API {
    [Shared]
    [SerialVersionUID(1513252653655L)]
    public class PremiumAccountQuestComponent : Component {
        public Date EndDate { get; set; }
    }
}