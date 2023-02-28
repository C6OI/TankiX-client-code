using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507538648077L)]
    public class AcceptInviteToSquadEvent : Event {
        public long FromUserId { get; set; }

        public long EngineId { get; set; }
    }
}