using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1510641456884L)]
    public class RejectRequestToSquadEvent : Event {
        public long FromUserId { get; set; }

        public long SquadId { get; set; }

        public long SquadEngineId { get; set; }
    }
}