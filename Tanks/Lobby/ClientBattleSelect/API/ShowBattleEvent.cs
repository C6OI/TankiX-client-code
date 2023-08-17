using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ShowBattleEvent : Event {
        public ShowBattleEvent() { }

        public ShowBattleEvent(long battleId) => BattleId = battleId;

        public long BattleId { get; set; }
    }
}