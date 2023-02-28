using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507727447201L)]
    public class ChangeSquadLeaderEvent : Event {
        public long NewLeaderUserId { get; set; }
    }
}