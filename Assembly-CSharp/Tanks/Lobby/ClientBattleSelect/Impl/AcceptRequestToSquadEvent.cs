using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507799982015L)]
    public class AcceptRequestToSquadEvent : Event {
        public long FromUserId { get; set; }

        public long SquadId { get; set; }

        public long SquadEngineId { get; set; }
    }
}