using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientFriends.Impl {
    public class UserLabelFriendsSystem : ECSSystem {
        [OnEventFire]
        public void SetFriendColor(NodeAddedEvent e, UserFriendNode user,
            [Context] [Combine] [JoinByUser] UserLabelNode userLabel) =>
            userLabel.uidIndicator.Color = userLabel.uidColor.FriendColor;

        [OnEventFire]
        public void SetNormalColor(NodeRemoveEvent e, UserFriendNode user, [Combine] [JoinByUser] UserLabelNode userLabel) =>
            userLabel.uidIndicator.Color = userLabel.uidColor.Color;

        public class UserLabelNode : Node {
            public UidColorComponent uidColor;

            public UidIndicatorComponent uidIndicator;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;
        }

        public class UserFriendNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public UserComponent user;

            public UserGroupComponent userGroup;
        }
    }
}