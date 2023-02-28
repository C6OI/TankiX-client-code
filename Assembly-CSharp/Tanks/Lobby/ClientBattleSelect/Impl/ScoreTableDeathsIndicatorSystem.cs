using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableDeathsIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetDeaths(NodeAddedEvent e, DeathsNode deaths, [Context] [JoinByUser] UserNode user) {
            deaths.scoreTableDeathsIndicator.Deaths = user.roundUserStatistics.Deaths;
        }

        [OnEventFire]
        public void SetDeaths(RoundUserStatisticsUpdatedEvent e, UserNode user, [JoinByUser] DeathsNode deaths) {
            deaths.scoreTableDeathsIndicator.Deaths = user.roundUserStatistics.Deaths;
        }

        public class DeathsNode : Node {
            public ScoreTableDeathsIndicatorComponent scoreTableDeathsIndicator;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;
        }
    }
}