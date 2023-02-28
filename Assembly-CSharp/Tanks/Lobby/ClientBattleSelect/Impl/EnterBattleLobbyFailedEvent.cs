using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1547709389410L)]
    public class EnterBattleLobbyFailedEvent : Event {
        public bool AlreadyInLobby { get; set; }

        public bool LobbyIsFull { get; set; }
    }
}