using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513581047619L)]
    public class DurationUserItemComponent : Component {
        public Date EndDate { get; set; }
    }
}