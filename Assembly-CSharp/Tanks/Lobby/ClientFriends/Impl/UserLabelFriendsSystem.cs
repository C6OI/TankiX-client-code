using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class UserLabelFriendsSystem : ECSSystem {
        [OnEventFire]
        public void SetFriendColor(NodeAddedEvent e, AcceptedFriendNode user, [JoinByUser] [Context] [Combine] UserLabelNode userLabel) {
            userLabel.uidIndicator.Color = userLabel.uidColor.FriendColor;
        }

        [OnEventFire]
        public void SetNormalColor(NodeRemoveEvent e, AcceptedFriendNode user, [JoinByUser] [Combine] UserLabelNode userLabel) {
            userLabel.uidIndicator.Color = userLabel.uidColor.Color;
        }

        [OnEventFire]
        public void HiglightFriend(NodeAddedEvent e, AcceptedFriendNode user, [JoinByUser] [Context] [Combine] UserLabelWithHighlightningNode userLabel) {
            userLabel.uidIndicator.FontStyle = userLabel.uidHighlighting.friend;
        }

        [OnEventFire]
        public void RemoveHiglightning(NodeRemoveEvent e, UserFriendNode user, [JoinByUser] [Context] [Combine] UserLabelWithHighlightningNode userLabel) {
            userLabel.uidIndicator.FontStyle = userLabel.uidHighlighting.normal;
        }

        [OnEventFire]
        public void EnableInBattleIcon(NodeAddedEvent e, UserInBattleNode user, [JoinByUser] [Context] [Combine] UserLabelNode userLabel) {
            if (userLabel.userLabel.AllowInBattleIcon && userLabel.userLabel.inBattleIcon != null) {
                userLabel.userLabel.inBattleIcon.SetActive(true);
            }
        }

        [OnEventFire]
        public void DisableInBattleIcon(NodeRemoveEvent e, UserInBattleNode user, [JoinByUser] [Combine] UserLabelNode userLabel) {
            if (userLabel.userLabel.AllowInBattleIcon && userLabel.userLabel.inBattleIcon != null) {
                userLabel.userLabel.inBattleIcon.SetActive(false);
            }
        }

        [OnEventFire]
        public void EnableInBattleState(NodeAddedEvent e, UserInBattleNode user, [JoinByUser] [Context] [Combine] UserLabelStateNode userLabel) {
            userLabel.userLabelState.UserInBattle();

            if (userLabel.Entity.HasComponent<BattleGroupComponent>()) {
                userLabel.Entity.RemoveComponent<BattleGroupComponent>();
            }

            user.battleGroup.Attach(userLabel.Entity);
        }

        [OnEventFire]
        public void DisableInBattleState(NodeRemoveEvent e, UserInBattleNode user, [JoinByUser] [Context] [Combine] UserLabelStateNode userLabel) {
            userLabel.userLabelState.UserOutBattle(user.Entity.HasComponent<UserOnlineComponent>());
        }

        [OnEventFire]
        public void HideIncomingFriendButtons(NodeAddedEvent e, AcceptedFriendNode friend, [JoinByUser] [Context] [Combine] FriendLabelNode userLabel) {
            userLabel.incomingFriendButtons.IsIncoming = false;
            userLabel.outgoingFriendButtons.IsOutgoing = false;
        }

        [OnEventFire]
        public void ShowIncomingFriendButtons(NodeAddedEvent e, IncomingFriendNode friend, [JoinByUser] [Context] [Combine] FriendLabelNode userLabel) {
            userLabel.incomingFriendButtons.IsIncoming = true;
        }

        [OnEventFire]
        public void ShowOutgoingFriendButton(NodeAddedEvent e, OutgoingFriendNode friend, [JoinByUser] [Context] [Combine] FriendLabelNode userLabel) {
            userLabel.outgoingFriendButtons.IsOutgoing = true;
        }

        public class UserLabelNode : Node {
            public UidColorComponent uidColor;

            public UidIndicatorComponent uidIndicator;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;
        }

        public class UserLabelWithHighlightningNode : UserLabelNode {
            public UidHighlightingComponent uidHighlighting;
        }

        public class UserLabelStateNode : Node {
            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public UserLabelStateComponent userLabelState;
        }

        public class UserFriendNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class IncomingFriendNode : UserFriendNode {
            public IncommingFriendComponent incommingFriend;
        }

        public class AcceptedFriendNode : UserFriendNode {
            public AcceptedFriendComponent acceptedFriend;
        }

        public class OutgoingFriendNode : UserFriendNode {
            public OutgoingFriendComponent outgoingFriend;
        }

        public class UserInBattleNode : UserFriendNode {
            public BattleGroupComponent battleGroup;
        }

        public class FriendLabelNode : Node {
            public IncomingFriendButtonsComponent incomingFriendButtons;

            public OutgoingFriendButtonsComponent outgoingFriendButtons;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;
        }
    }
}