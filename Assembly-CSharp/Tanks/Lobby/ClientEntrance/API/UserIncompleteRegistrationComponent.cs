using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1482675132842L)]
    public class UserIncompleteRegistrationComponent : Component {
        public bool FirstBattleDone { get; set; }
    }
}