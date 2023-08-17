using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine.UI;

namespace Lobby.ClientFriends.Impl {
    public class FriendsScreenSystem : ECSSystem {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void SearchFieldSetFocusOnLoad(NodeAddedEvent e, SingleNode<FriendsScreenComponent> screen,
            [JoinByScreen] SearchUserInputFieldNode searchField) {
            screen.component.SearchButton.SetActive(true);
            searchField.inputField.InputField.Select();
            searchField.inputField.InputField.ActivateInputField();
        }

        [OnEventFire]
        public void HideAllButtons(NodeRemoveEvent e, SelectedFriendUINode friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> screen) {
            screen.component.SearchButton.SetActive(false);
            screen.component.ProfileButton.SetActive(false);
            screen.component.AcceptButton.SetActive(false);
            screen.component.RejectButton.SetActive(false);
            screen.component.RevokeButton.SetActive(false);
            screen.component.RemoveButton.SetActive(false);
        }

        [OnEventFire]
        public void ShowSearchUserButtons(EventSystemOnPointerClickEvent e, SearchUserInputFieldNode searchField,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList,
            [JoinByScreen] SingleNode<FriendsScreenComponent> screen) {
            friendsList.component.gameObject.GetComponent<ToggleGroup>().SetAllTogglesOff();
            screen.component.SearchButton.SetActive(true);
        }

        [OnEventFire]
        public void ValidateSearchUserField(InputFieldValueChangedEvent e, SearchUserInputFieldNode searchField) =>
            searchField.esm.Esm.ChangeState<InputFieldStates.NormalState>();

        [OnEventFire]
        public void HideSearchUserButtons(NodeAddedEvent e, SelectedFriendUINode friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> screen) => screen.component.SearchButton.SetActive(false);

        [OnEventFire]
        public void RequestUserId(ButtonClickEvent e, SingleNode<SearchUserButtonComponent> button,
            [JoinByScreen] SearchUserInputFieldNode searchField,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession) {
            SearchUserIdByUidEvent searchUserIdByUidEvent = new();
            searchUserIdByUidEvent.Uid = searchField.searchUserInputField.SearchString;
            ScheduleEvent(searchUserIdByUidEvent, clientSession);
            button.component.gameObject.SetInteractable(false);
            searchField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
        }

        [OnEventFire]
        public void ResponseUserId(SerachUserIdByUidResultEvent e, SingleNode<ClientSessionComponent> clientSession,
            [JoinAll] SingleNode<FriendsScreenComponent> screen, [JoinByScreen] SingleNode<FriendsListComponent> friendsList,
            [JoinByScreen] SearchUserInputFieldNode searchField,
            [JoinByScreen] SingleNode<SearchUserButtonComponent> button) {
            button.component.gameObject.SetInteractable(true);

            if (e.Found) {
                ScheduleEvent(new ShowProfileScreenEvent(e.UserId), EngineService.EntityStub);
                return;
            }

            searchField.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            friendsList.component.ToggleGroup.SetAllTogglesOff();
            searchField.inputField.InputField.Select();
            searchField.inputField.InputField.ActivateInputField();
        }

        [OnEventFire]
        public void ShowAcceptedFriendButtons(NodeAddedEvent e, SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<AcceptedFriendComponent> friend, [JoinAll] SingleNode<FriendsScreenComponent> screen) {
            screen.component.ProfileButton.SetActive(true);
            screen.component.RemoveButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowIncommingFriendButtons(NodeAddedEvent e, SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<IncommingFriendComponent> friend, [JoinAll] SingleNode<FriendsScreenComponent> screen) {
            screen.component.ProfileButton.SetActive(true);
            screen.component.AcceptButton.SetActive(true);
            screen.component.RejectButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowOutgoingFriendButtons(NodeAddedEvent e, SelectedFriendUINode friendUI,
            [JoinByUser] SingleNode<OutgoingFriendComponent> friend, [JoinAll] SingleNode<FriendsScreenComponent> screen) {
            screen.component.ProfileButton.SetActive(true);
            screen.component.RevokeButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowProfile(ButtonClickEvent e, SingleNode<FriendProfileButtonComponent> button,
            [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<UserComponent> friend) =>
            ScheduleEvent(new ShowProfileScreenEvent(friend.Entity.Id), friend.Entity);

        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendButtonComponent> button,
            [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<IncommingFriendComponent> friend,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent(new AcceptFriendEvent(friend.Entity), user);

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<RejectFriendButtonComponent> button,
            [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<IncommingFriendComponent> friend,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent(new RejectFriendEvent(friend.Entity), user);

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendButtonComponent> button,
            [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<OutgoingFriendComponent> friend,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent(new RevokeFriendEvent(friend.Entity), user);

        [OnEventFire]
        public void RemoveFriend(ConfirmButtonClickYesEvent e, SingleNode<BreakOffFriendButtonComponent> button,
            [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<AcceptedFriendComponent> friend,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent(new BreakOffFriendEvent(friend.Entity), user);

        [OnEventFire]
        public void SetDefaultNotification(NodeAddedEvent e, SingleNode<FriendsScreenComponent> friendsScreen,
            [Context] [JoinByScreen] SingleNode<FriendsListComponent> friendsList) {
            friendsList.component.PossibleFriendsHeader.SetActive(false);
            friendsList.component.EmptyListNotify.SetActive(true);
        }

        [OnEventFire]
        public void HideEmptyListNotification(NodeAddedEvent e, SingleNode<FriendsListItemComponent> friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList) =>
            friendsList.component.EmptyListNotify.SetActive(false);

        [OnEventFire]
        public void ShowEmptyListNotification(NodeRemoveEvent e, SingleNode<FriendsListItemComponent> friendUI,
            [JoinAll] ICollection<SingleNode<FriendsListItemComponent>> friendUIs,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList) {
            if (friendUIs.Count == 1) {
                friendsList.component.EmptyListNotify.SetActive(true);
            }
        }

        [OnEventFire]
        public void ShowAcceptedFriendsHeader(NodeAddedEvent e, FriendUINode friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList, [JoinAll] SingleNode<FriendsComponent> friends) {
            if (friends.component.AcceptedFriendsIds.Count > 0) {
                friendsList.component.AcceptedFriendsHeader.SetActive(true);
            }
        }

        [OnEventFire]
        public void ShowPossibleFriendsHeader(NodeAddedEvent e, FriendUINode friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList, [JoinAll] SingleNode<FriendsComponent> friends) {
            if (friends.component.IncommingFriendsIds.Count + friends.component.OutgoingFriendsIds.Count > 0) {
                friendsList.component.PossibleFriendsHeader.SetActive(true);
            }
        }

        [OnEventFire]
        public void HideAcceptedFriendsHeader(NodeRemoveEvent e, FriendUINode friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList, [JoinAll] SingleNode<FriendsComponent> friends) {
            if (friends.component.AcceptedFriendsIds.Count == 0) {
                friendsList.component.AcceptedFriendsHeader.SetActive(false);
            }
        }

        [OnEventFire]
        public void HidePossibleFriendsHeader(NodeRemoveEvent e, FriendUINode friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList, [JoinAll] SingleNode<FriendsComponent> friends) {
            if (friends.component.IncommingFriendsIds.Count + friends.component.OutgoingFriendsIds.Count == 0) {
                friendsList.component.PossibleFriendsHeader.SetActive(false);
            }
        }

        public class FriendUINode : Node {
            public FriendsListItemComponent friendsListItem;
            public UserGroupComponent userGroup;

            public UserLoadedComponent userLoaded;
        }

        public class SelectedFriendUINode : FriendUINode {
            public SelectedListItemComponent selectedListItem;
        }

        public class SearchUserInputFieldNode : Node {
            public ESMComponent esm;
            public InputFieldComponent inputField;

            public SearchUserInputFieldComponent searchUserInputField;
        }
    }
}