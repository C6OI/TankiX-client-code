using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class LobbyFriendsScreenSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void RequestUserId(ButtonClickEvent e, SingleNode<SearchUserButtonComponent> button, [JoinByScreen] SearchUserInputFieldNode searchField,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession) {
            SearchUserIdByUidEvent searchUserIdByUidEvent = new();
            searchUserIdByUidEvent.Uid = searchField.searchUserInputField.SearchString;
            ScheduleEvent(searchUserIdByUidEvent, clientSession);
            button.component.gameObject.SetInteractable(false);
            searchField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
        }

        [OnEventFire]
        public void ValidateSearchUserField(InputFieldValueChangedEvent e, SearchUserInputFieldNode searchField) {
            searchField.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        [OnEventFire]
        public void ResponseUserId(SerachUserIdByUidResultEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<AddFriendDialogComponent> dialog,
            [JoinByScreen] SearchUserInputFieldNode searchField, [JoinByScreen] SingleNode<SearchUserButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            button.component.gameObject.SetInteractable(true);

            if (e.Found) {
                ScheduleEvent(new RequestLoadUserProfileEvent(e.UserId), selfUser);
            } else {
                searchField.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            }
        }

        [OnEventFire]
        public void AttachProfileScreenToUserGroup(UserProfileLoadedEvent e, RemoteUserNode remoteUser, [JoinAll] SingleNode<AddFriendDialogComponent> dialog,
            [JoinByScreen] SearchUserInputFieldNode searchField, [JoinByScreen] SingleNode<SearchUserButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            ScheduleEvent(new RequestFriendEvent(remoteUser.Entity), selfUser);
            ScheduleEvent<FriendRequestSentEvent>(selfUser);
            dialog.component.Hide();
        }

        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, SingleNode<FriendSentNotificationComponent> notification, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            notification.Entity.AddComponent(new NotificationMessageComponent(notification.component.Message));
        }

        [OnEventFire]
        public void ShowProfile(ButtonClickEvent e, SingleNode<FriendProfileButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<UserComponent> friend) {
            ScheduleEvent(new ShowProfileScreenEvent(friend.Entity.Id), friend.Entity);
        }

        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<IncommingFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent(new AcceptFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<RejectFriendButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<IncommingFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent(new RejectFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendRequestButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<OutgoingFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent(new RevokeFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void RemoveFriend(ConfirmButtonClickYesEvent e, SingleNode<BreakOffFriendButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<AcceptedFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent(new BreakOffFriendEvent(friend.Entity), user);
        }

        public class FriendUINode : Node {
            public FriendsListItemComponent friendsListItem;
            public UserGroupComponent userGroup;

            public UserLoadedComponent userLoaded;
        }

        public class SelectedFriendUINode : FriendUINode {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SearchUserInputFieldNode : Node {
            public ESMComponent esm;
            public InputFieldComponent inputField;

            public SearchUserInputFieldComponent searchUserInputField;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }
    }
}