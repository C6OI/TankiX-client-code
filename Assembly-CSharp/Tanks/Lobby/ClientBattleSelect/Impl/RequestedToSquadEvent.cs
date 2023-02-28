using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507799564788L)]
    public class RequestedToSquadEvent : Event {
        public string UserUid { get; set; }

        public long FromUserId { get; set; }

        public long SquadId { get; set; }

        public long EngineId { get; set; }
    }
}