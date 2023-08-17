using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleScoreLimitIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetScoreLimit(NodeAddedEvent e, BattleScoreLimitIndicatorNode battleScoreLimitIndicator,
            [Context] [JoinByBattle] BattleLimitNode battleLimit) =>
            battleScoreLimitIndicator.battleScoreLimitIndicator.ScoreLimit = battleLimit.scoreLimit.ScoreLimit;

        public class BattleScoreLimitIndicatorNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleScoreLimitIndicatorComponent battleScoreLimitIndicator;
        }

        public class BattleLimitNode : Node {
            public BattleGroupComponent battleGroup;

            public ScoreLimitComponent scoreLimit;
        }
    }
}