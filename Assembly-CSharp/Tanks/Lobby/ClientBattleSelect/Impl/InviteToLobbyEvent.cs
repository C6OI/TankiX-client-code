using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1497002374017L)]
    public class InviteToLobbyEvent : Event {
        public long[] InvitedUserIds { get; set; }
    }
}