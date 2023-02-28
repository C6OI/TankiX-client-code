using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class BattleResultsComponent : Component {
        public BattleResultForClient ResultForClient { get; set; }
    }
}