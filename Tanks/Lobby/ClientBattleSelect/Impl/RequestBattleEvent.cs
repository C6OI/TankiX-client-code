using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1452777153835L)]
    [Shared]
    public class RequestBattleEvent : Event {
        public RequestBattleEvent() { }

        public RequestBattleEvent(long battleId) => BattleId = battleId;

        public long BattleId { get; set; }
    }
}