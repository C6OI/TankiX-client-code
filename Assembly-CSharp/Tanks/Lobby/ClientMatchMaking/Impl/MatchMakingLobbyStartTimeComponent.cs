using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [Shared]
    [SerialVersionUID(1496833452921L)]
    public class MatchMakingLobbyStartTimeComponent : Component {
        public Date StartTime { get; set; }
    }
}