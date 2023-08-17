using System;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lobby.ClientFriends.Impl {
    public class FriendsBuilderSystem : ECSSystem {
        [OnEventFire]
        public void RequestSortedFriends(NodeAddedEvent e, SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinAll] SingleNode<ClientSessionComponent> session) => ScheduleEvent<LoadSortedFriendsIdsEvent>(session);

        [OnEventFire]
        public void CreateUIsForFriends(SortedFriendsIdsLoadedEvent e, SingleNode<ClientSessionComponent> session,
            [JoinByUser] SingleNode<FriendsComponent> friends, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList) => e.FriendsIds.ForEach(delegate(long id) {
            CreateFriendUI(id,
                friendsList.component.FriendsListItem,
                GetParentByUserId(id, friends.component, friendsList.component));
        });

        void CreateFriendUI(long userId, GameObject itemPrefab, GameObject parent) {
            GameObject gameObject = Object.Instantiate(itemPrefab);
            gameObject.transform.SetParent(parent.GetComponent<RectTransform>(), false);
            gameObject.GetComponent<ToggleListItemComponent>().AttachToParentToggleGroup();
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new LoadUserComponent(userId));
            entity.AddComponent(new UserGroupComponent(userId));
        }

        GameObject GetParentByUserId(long userId, FriendsComponent friends, FriendsListComponent friendsList) {
            if (friends.AcceptedFriendsIds.Contains(userId)) {
                return friendsList.FriendsAcceptedList;
            }

            if (friends.IncommingFriendsIds.Contains(userId)) {
                return friendsList.FriendsIncommingList;
            }

            if (friends.OutgoingFriendsIds.Contains(userId)) {
                return friendsList.FriendsOutgoingList;
            }

            throw new ArgumentOutOfRangeException();
        }

        [OnEventFire]
        public void SetUserLabel(NodeAddedEvent e, FriendUIWithUserNode friendUI) {
            GameObject gameObject = new UserLabelBuilder(friendUI.userGroup.Key).Build();
            gameObject.transform.SetParent(friendUI.friendsListItem.UserLabelContainer.gameObject.transform, false);
        }

        [OnEventFire]
        public void AddNewFriend(FriendAddedBaseEvent e, SingleNode<FriendsComponent> friends,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList) => CreateFriendUI(e.FriendId,
            friendsList.component.FriendsListItem,
            GetParentByUserId(e.FriendId, friends.component, friendsList.component));

        [OnEventFire]
        public void RemoveOutdatedUI(NodeAddedEvent e, FriendUI newFriendUI, [JoinByUser] [Combine] FriendUI oldFriendUI) {
            if (newFriendUI.Entity.Id != oldFriendUI.Entity.Id) {
                Object.Destroy(oldFriendUI.friendsListItem.gameObject);
            }
        }

        [OnEventFire]
        public void RemoveFriendUI(NodeRemoveEvent e, SingleNode<FriendComponent> friend,
            [JoinByUser] SingleNode<FriendsListItemComponent> friendUI) => Object.Destroy(friendUI.component.gameObject);

        [OnEventFire]
        public void CleanList(NodeRemoveEvent e, SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinByScreen] SingleNode<FriendsListComponent> friendsList) {
            friendsList.component.FriendsAcceptedList.transform.DestroyChildren();
            friendsList.component.FriendsIncommingList.transform.DestroyChildren();
            friendsList.component.FriendsOutgoingList.transform.DestroyChildren();
        }

        public class FriendUI : Node {
            public FriendsListItemComponent friendsListItem;

            public UserGroupComponent userGroup;
        }

        public class FriendUIWithUserNode : FriendUI {
            public UserLoadedComponent userLoaded;
        }
    }
}