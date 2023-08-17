using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class UserLabelUidSystem : ECSSystem {
        [OnEventFire]
        public void SetUid(NodeAddedEvent e, UserLabelNode userLabel, [JoinByUser] [Context] UserNode user) =>
            userLabel.uidIndicator.Uid = user.userUid.Uid;

        public class UserLabelNode : Node {
            public UidIndicatorComponent uidIndicator;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }
    }
}