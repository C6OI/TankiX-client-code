using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1547616531111L)]
    public class ConnectToCustomLobbyEvent : Event {
        public long LobbyId { get; set; }
    }
}