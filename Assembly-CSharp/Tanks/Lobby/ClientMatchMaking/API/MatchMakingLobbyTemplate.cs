using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [SerialVersionUID(1495541167479L)]
    public interface MatchMakingLobbyTemplate : BattleLobbyTemplate, Template { }
}