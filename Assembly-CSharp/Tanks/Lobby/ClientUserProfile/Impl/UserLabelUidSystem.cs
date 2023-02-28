using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserLabelUidSystem : ECSSystem {
        [OnEventFire]
        public void SetUid(NodeAddedEvent e, [Combine] UserLabelNode userLabel, [Context] [JoinByUser] UserNode user) {
            userLabel.uidIndicator.Uid = user.userUid.Uid;
        }

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