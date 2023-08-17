using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class RankIconSystem : ECSSystem {
        [OnEventFire]
        public void SetIcon(NodeAddedEvent e, [Combine] RankIconNode rankIcon,
            [Context] [JoinByUser] UserRankNode userRank) => rankIcon.rankIcon.SetRank(userRank.userRank.Rank + 1);

        [OnEventFire]
        public void UpdateRank(UpdateRankEvent e, UserRankNode userRank, [JoinByUser] [Combine] RankIconNode rankIcon) =>
            rankIcon.rankIcon.SetRank(userRank.userRank.Rank + 1);

        public class RankIconNode : Node {
            public RankIconComponent rankIcon;

            public UserGroupComponent userGroup;
        }

        public class UserRankNode : Node {
            public UserGroupComponent userGroup;
            public UserRankComponent userRank;
        }
    }
}