using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ReputationBattleResultsComponent : Component {
        public int Delta { get; set; }

        public bool UnfairMatching { get; set; }
    }
}