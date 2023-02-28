using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1502716170372L)]
    public class UserReputationComponent : Component {
        public double Reputation { get; set; }
    }
}