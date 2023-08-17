using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class UserLabelSystem : ECSSystem {
        [OnEventComplete]
        public void AddUserModerator(NodeAddedEvent e, UserModeratorNode user,
            [Combine] [Context] [JoinByUser] UserLabelNode userLabel) =>
            userLabel.uidIndicator.Color = userLabel.uidColor.ModeratorColor;

        [OnEventFire]
        public void UserOffline(NodeAddedEvent e, UserOfflineNode user,
            [Combine] [Context] [JoinByUser] UserLabelNode userLabel) => MarkUserAvatarAsOffline(userLabel);

        [OnEventFire]
        public void UserOnline(NodeAddedEvent e, UserOnlineNode user,
            [JoinByUser] [Context] [Combine] UserLabelNode userLabel) {
            Image component = userLabel.userLabelAvatar.AvatarImage.GetComponent<Image>();
            component.color = userLabel.userLabelAvatar.OnlineColor;
        }

        [OnEventFire]
        public void UserWentOffline(NodeRemoveEvent e, UserOnlineNode user,
            [JoinByUser] [Combine] UserLabelNode userLabel) => MarkUserAvatarAsOffline(userLabel);

        void MarkUserAvatarAsOffline(UserLabelNode userLabel) {
            Image component = userLabel.userLabelAvatar.AvatarImage.GetComponent<Image>();
            component.color = userLabel.userLabelAvatar.OfflineColor;
        }

        public class UserLabelNode : Node {
            public RankIconComponent rankIcon;

            public UidColorComponent uidColor;

            public UidIndicatorComponent uidIndicator;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public UserLabelAvatarComponent userLabelAvatar;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class UserOnlineNode : UserNode {
            public UserOnlineComponent userOnline;
        }

        [Not(typeof(UserOnlineComponent))]
        public class UserOfflineNode : UserNode { }

        public class UserModeratorNode : UserNode {
            public ModeratorComponent moderator;
        }
    }
}