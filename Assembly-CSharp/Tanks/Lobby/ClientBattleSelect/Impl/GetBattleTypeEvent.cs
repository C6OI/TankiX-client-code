using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class GetBattleTypeEvent : Event {
        public bool WithCashback { get; set; }

        public BattleResultsAwardsScreenComponent.BattleTypes BattleType { get; set; }
    }
}