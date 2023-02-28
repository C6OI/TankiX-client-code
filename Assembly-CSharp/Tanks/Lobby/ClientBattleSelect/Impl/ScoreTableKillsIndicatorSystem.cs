using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableKillsIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetKills(NodeAddedEvent e, [Combine] KillsNode kills, [Context] [JoinByUser] UserNode user) {
            kills.scoreTableKillsIndicator.Kills = user.roundUserStatistics.Kills;
        }

        [OnEventFire]
        public void SetKills(RoundUserStatisticsUpdatedEvent e, UserNode user, [Combine] [JoinByUser] KillsNode kills) {
            kills.scoreTableKillsIndicator.Kills = user.roundUserStatistics.Kills;
        }

        public class KillsNode : Node {
            public ScoreTableKillsIndicatorComponent scoreTableKillsIndicator;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;
        }
    }
}