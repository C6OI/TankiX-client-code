using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class ClientMatchMakingLobbyStartTimeComponent : Component {
        public Date StartTime { get; set; }
    }
}