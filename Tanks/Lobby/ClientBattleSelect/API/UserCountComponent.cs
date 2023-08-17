using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1436520497855L)]
    public class UserCountComponent : Component {
        public int UserCount { get; set; }
    }
}