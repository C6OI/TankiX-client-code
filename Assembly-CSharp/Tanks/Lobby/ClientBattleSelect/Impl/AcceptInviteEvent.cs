using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1497349612322L)]
    public class AcceptInviteEvent : Event {
        public long lobbyId { get; set; }

        public long engineId { get; set; }
    }
}