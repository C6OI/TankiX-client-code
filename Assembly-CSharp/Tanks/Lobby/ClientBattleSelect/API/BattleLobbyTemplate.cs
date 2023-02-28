using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1498460800928L)]
    public interface BattleLobbyTemplate : Template {
        [AutoAdded]
        BattleLobbyComponent battleLobby();

        BattleLobbyGroupComponent battleLobbyGroup();
    }
}