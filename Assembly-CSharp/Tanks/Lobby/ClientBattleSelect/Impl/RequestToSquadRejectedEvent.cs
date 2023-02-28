using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1508315556885L)]
    public class RequestToSquadRejectedEvent : Event {
        public RejectRequestToSquadReason Reason { get; set; }

        public long RequestReceiverId { get; set; }
    }
}