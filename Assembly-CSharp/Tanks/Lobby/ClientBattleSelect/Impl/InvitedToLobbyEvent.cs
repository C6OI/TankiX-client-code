using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1499233137837L)]
    public class InvitedToLobbyEvent : Event {
        public string userUid { get; set; }

        public long lobbyId { get; set; }

        public long engineId { get; set; }
    }
}