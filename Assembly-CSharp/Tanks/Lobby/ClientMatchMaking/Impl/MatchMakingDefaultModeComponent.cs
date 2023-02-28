using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [Shared]
    [SerialVersionUID(1499242904641L)]
    public class MatchMakingDefaultModeComponent : Component {
        public int MinimalBattles { get; set; }
    }
}