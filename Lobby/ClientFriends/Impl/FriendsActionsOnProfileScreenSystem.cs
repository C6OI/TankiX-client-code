using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientFriends.Impl {
    public class FriendsActionsOnProfileScreenSystem : ECSSystem {
        [OnEventFire]
        public void ShowRemoveFriendButton(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen,
            [Context] [JoinByUser] FriendUserNode friendUser) {
            profileScreen.profileScreen.RemoveFriendButton.SetActive(true);
            profileScreen.profileScreen.RequestFriendButton.SetActive(false);
        }

        [OnEventFire]
        public void HideRemoveFriendButton(NodeRemoveEvent e, FriendUserNode friendUser,
            [JoinByUser] ProfileScreenWithUserGroupNode profileScreen) {
            profileScreen.profileScreen.RemoveFriendButton.SetActive(false);
            profileScreen.profileScreen.RequestFriendButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowRequestFriendButtonIfNeed(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen,
            [JoinByUser] PossibleFriendNode possibleFriend, [JoinAll] SelfUserNode selfUser) {
            bool active = !selfUser.Entity.Equals(possibleFriend.Entity) &&
                          !selfUser.friends.AcceptedFriendsIds.Contains(possibleFriend.Entity.Id) &&
                          !selfUser.friends.IncommingFriendsIds.Contains(possibleFriend.Entity.Id) &&
                          !selfUser.friends.OutgoingFriendsIds.Contains(possibleFriend.Entity.Id);

            profileScreen.profileScreen.RequestFriendButton.SetActive(active);
        }

        [OnEventFire]
        public void ShowIncommingFriendButtons(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen,
            [Context] [JoinByUser] IncommingFriendNode incommingFriend) {
            profileScreen.profileScreen.AdditionalMessageForButtonText.text =
                profileScreen.profileScreenLocalization.OfferFriendShipText;

            profileScreen.profileScreen.AdditionalMessageForButtonText.gameObject.SetActive(true);
            profileScreen.profileScreen.AcceptFriendButton.SetActive(true);
            profileScreen.profileScreen.RejectFriendButton.SetActive(true);
            profileScreen.profileScreen.RequestFriendButton.SetActive(false);
        }

        [OnEventFire]
        public void HideIncommingFriendButtons(NodeRemoveEvent e, IncommingFriendNode incommingFriend,
            [JoinByUser] ProfileScreenWithUserGroupNode profileScreen) {
            profileScreen.profileScreen.AcceptFriendButton.SetActive(false);
            profileScreen.profileScreen.RejectFriendButton.SetActive(false);
            profileScreen.profileScreen.AdditionalMessageForButtonText.gameObject.SetActive(false);
            profileScreen.profileScreen.RequestFriendButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowOutgoingFriendButtons(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen,
            [JoinByUser] [Context] OutgoingFriendNode outgoingFriend) {
            profileScreen.profileScreen.AdditionalMessageForButtonText.text =
                profileScreen.profileScreenLocalization.FriendRequestSentText;

            profileScreen.profileScreen.AdditionalMessageForButtonText.gameObject.SetActive(true);
            profileScreen.profileScreen.RevokeFriendButton.SetActive(true);
            profileScreen.profileScreen.RequestFriendButton.SetActive(false);
        }

        [OnEventFire]
        public void HideOutgoingFriendButtons(NodeRemoveEvent e, OutgoingFriendNode outgoingFriend,
            [JoinByUser] ProfileScreenWithUserGroupNode profileScreen) {
            profileScreen.profileScreen.AdditionalMessageForButtonText.gameObject.SetActive(false);
            profileScreen.profileScreen.RevokeFriendButton.SetActive(false);
            profileScreen.profileScreen.RequestFriendButton.SetActive(true);
        }

        [OnEventFire]
        public void RemoveFriend(ConfirmButtonClickYesEvent e, SingleNode<BreakOffFriendButtonComponent> button,
            [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] FriendUserNode friend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) =>
            ScheduleEvent(new BreakOffFriendEvent(friend.Entity), selfUser);

        [OnEventFire]
        public void RequestFriend(ButtonClickEvent e, SingleNode<RequestFriendButtonComponent> button,
            [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] NotFriendUserNode notFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) =>
            ScheduleEvent(new RequestFriendEvent(notFriend.Entity), selfUser);

        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendButtonComponent> button,
            [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] IncommingFriendNode incommingFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) =>
            ScheduleEvent(new AcceptFriendEvent(incommingFriend.Entity), selfUser);

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<RejectFriendButtonComponent> button,
            [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] IncommingFriendNode incommingFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) =>
            ScheduleEvent(new RejectFriendEvent(incommingFriend.Entity), selfUser);

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendButtonComponent> button,
            [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] OutgoingFriendNode outgoingFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) =>
            ScheduleEvent(new RevokeFriendEvent(outgoingFriend.Entity), selfUser);

        public class SelfUserNode : Node {
            public FriendsComponent friends;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class PossibleFriendNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class FriendUserNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(AcceptedFriendComponent))]
        public class NotFriendUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class IncommingFriendNode : Node {
            public IncommingFriendComponent incommingFriend;

            public UserGroupComponent userGroup;
        }

        public class OutgoingFriendNode : Node {
            public OutgoingFriendComponent outgoingFriend;

            public UserGroupComponent userGroup;
        }

        public class ProfileScreenWithUserGroupNode : Node {
            public ActiveScreenComponent activeScreen;
            public ProfileScreenComponent profileScreen;

            public ProfileScreenLocalizationComponent profileScreenLocalization;

            public UserGroupComponent userGroup;
        }
    }
}