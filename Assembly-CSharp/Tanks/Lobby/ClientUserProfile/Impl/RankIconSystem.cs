using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class RankIconSystem : ECSSystem {
        [OnEventFire]
        public void SetIcon(NodeAddedEvent e, [Combine] RankIconNode rankIcon, [Context] [JoinByUser] UserRankNode userRank) {
            rankIcon.rankIcon.SetRank(userRank.userRank.Rank);
        }

        [OnEventFire]
        public void UpdateRank(UpdateRankEvent e, UserRankNode userRank, [JoinByUser] [Combine] RankIconNode rankIcon) {
            rankIcon.rankIcon.SetRank(userRank.userRank.Rank);
        }

        [OnEventFire]
        public void UpdateRank(UpdateRankEvent e, [Combine] SelfUserRankNode userRank, [Combine] [JoinAll] SingleNode<SelfUserAvatarComponent> selfUserAvatar) {
            selfUserAvatar.component.SetRank(userRank.userRank.Rank);
        }

        public class RankIconNode : Node {
            public RankIconComponent rankIcon;

            public UserGroupComponent userGroup;
        }

        public class UserRankNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;
        }

        public class SelfUserRankNode : UserRankNode {
            public SelfUserComponent selfUser;
        }
    }
}