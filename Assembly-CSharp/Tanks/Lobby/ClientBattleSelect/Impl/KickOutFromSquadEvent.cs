using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507210730593L)]
    public class KickOutFromSquadEvent : Event {
        public long KickedOutUserId { get; set; }
    }
}