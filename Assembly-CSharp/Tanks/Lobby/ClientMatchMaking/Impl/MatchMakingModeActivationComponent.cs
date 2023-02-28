using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class MatchMakingModeActivationComponent : Component {
        public bool Active { get; set; }
    }
}