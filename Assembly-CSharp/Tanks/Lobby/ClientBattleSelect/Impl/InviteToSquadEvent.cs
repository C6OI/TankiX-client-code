using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507211574274L)]
    public class InviteToSquadEvent : Event {
        public long[] InvitedUsersIds { get; set; }
    }
}