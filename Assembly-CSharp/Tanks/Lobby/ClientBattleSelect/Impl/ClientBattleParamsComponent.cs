using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1498569137147L)]
    public class ClientBattleParamsComponent : Component {
        public ClientBattleParams Params { get; set; }
    }
}