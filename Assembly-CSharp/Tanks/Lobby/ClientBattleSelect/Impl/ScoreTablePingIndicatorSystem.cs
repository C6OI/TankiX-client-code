using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTablePingIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetPing(NodeAddedEvent e, [Combine] PingIndicatorNode pingIndicator, [Context] [JoinByUser] UserNode user) {
            SetPing(pingIndicator, user);
        }

        [OnEventFire]
        public void SetPing(RoundUserStatisticsUpdatedEvent e, UserNode user, [Combine] [JoinByUser] PingIndicatorNode pingIndicator) {
            SetPing(pingIndicator, user);
        }

        void SetPing(PingIndicatorNode pingIndicator, UserNode user) { }

        public class PingIndicatorNode : Node {
            public ScoreTablePingIndicatorComponent scoreTablePingIndicator;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;
        }
    }
}