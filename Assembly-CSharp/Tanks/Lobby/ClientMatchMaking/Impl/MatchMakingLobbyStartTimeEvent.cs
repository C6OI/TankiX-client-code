using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [Shared]
    [SerialVersionUID(1499762071035L)]
    public class MatchMakingLobbyStartTimeEvent : Event {
        public Date StartTime { get; set; }
    }
}