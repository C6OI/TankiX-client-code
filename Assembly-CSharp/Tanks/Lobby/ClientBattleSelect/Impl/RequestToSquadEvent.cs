using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507792868618L)]
    public class RequestToSquadEvent : Event {
        public long ToUserId { get; set; }

        public long SquadId { get; set; }
    }
}