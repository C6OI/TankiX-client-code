using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableScoreIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetScore(NodeAddedEvent e, ScoreNode score, [Context] [JoinByUser] UserNode user) {
            score.scoreTableScoreIndicator.Score = user.roundUserStatistics.ScoreWithoutBonuses;
        }

        [OnEventFire]
        public void SetScore(RoundUserStatisticsUpdatedEvent e, UserNode user, [JoinByUser] ScoreNode score) {
            score.scoreTableScoreIndicator.Score = user.roundUserStatistics.ScoreWithoutBonuses;
        }

        public class ScoreNode : Node {
            public ScoreTableScoreIndicatorComponent scoreTableScoreIndicator;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;
        }
    }
}