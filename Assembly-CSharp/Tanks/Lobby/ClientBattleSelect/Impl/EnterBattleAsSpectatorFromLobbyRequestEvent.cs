using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1498554483631L)]
    public class EnterBattleAsSpectatorFromLobbyRequestEvent : Event {
        public EnterBattleAsSpectatorFromLobbyRequestEvent(long battleId) => BattleId = battleId;

        public long BattleId { get; set; }
    }
}