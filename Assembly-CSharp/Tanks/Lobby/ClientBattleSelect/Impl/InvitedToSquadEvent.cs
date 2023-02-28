using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507543176898L)]
    public class InvitedToSquadEvent : Event {
        public string UserUid { get; set; }

        public long FromUserId { get; set; }

        public long EngineId { get; set; }
    }
}