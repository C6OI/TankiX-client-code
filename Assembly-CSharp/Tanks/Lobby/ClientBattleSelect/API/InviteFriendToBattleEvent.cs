using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(635900246805239980L)]
    public class InviteFriendToBattleEvent : Event {
        public InviteFriendToBattleEvent(long battleId) => BattleId = battleId;

        public long BattleId { get; set; }
    }
}