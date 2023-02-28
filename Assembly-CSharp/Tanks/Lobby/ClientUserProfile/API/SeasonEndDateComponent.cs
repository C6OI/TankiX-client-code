using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1505728594733L)]
    public class SeasonEndDateComponent : Component {
        [ProtocolOptional] public Date EndDate { get; set; }
    }
}