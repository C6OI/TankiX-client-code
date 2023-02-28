using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1502713060357L)]
    public class LeagueConfigComponent : Component {
        public int LeagueIndex { get; set; }

        public double ReputationToEnter { get; set; }
    }
}