using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class RankNameSystem : ECSSystem {
        [OnEventComplete]
        public void SetRankName(NodeAddedEvent e, [Combine] RankNameNode rankName, [Context] [JoinByUser] UserNode user, [JoinAll] SingleNode<RanksNamesComponent> ranksNames) {
            rankName.rankName.RankName = ranksNames.component.Names[user.userRank.Rank];
        }

        [OnEventFire]
        public void UpdateRankName(UpdateRankEvent e, UserNode user, [JoinByUser] [Combine] RankNameNode rankName, [JoinAll] SingleNode<RanksNamesComponent> ranksNames) {
            rankName.rankName.RankName = ranksNames.component.Names[user.userRank.Rank];
        }

        public class RankNameNode : Node {
            public RankNameComponent rankName;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;
        }
    }
}