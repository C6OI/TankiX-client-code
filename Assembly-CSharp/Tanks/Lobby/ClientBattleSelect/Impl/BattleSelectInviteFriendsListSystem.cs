using System;
using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectInviteFriendsListSystem : ECSSystem {
        [OnEventFire]
        public void LoadAllAcceptedFriends(NodeAddedEvent e, SingleNode<InviteFriendsListComponent> inviteFriendsList, [JoinAll] SingleNode<FriendsComponent> friends) {
            inviteFriendsList.Entity.AddComponent(new LoadUsersComponent(friends.component.AcceptedFriendsIds));
        }

        [OnEventFire]
        public void CreateFriendsListItems(NodeAddedEvent e, UserFriendsLoadedNode userFriendsLoaded, [JoinAll] SingleNode<FriendsComponent> friends) {
            foreach (long acceptedFriendsId in friends.component.AcceptedFriendsIds) {
                ScheduleEvent<CreateInviteFriendListItemEvent>(Flow.Current.EntityRegistry.GetEntity(acceptedFriendsId));
            }
        }

        [OnEventFire]
        public void CreateFriendListItem(CreateInviteFriendListItemEvent e, OnlineFriendNode friend, [JoinSelf] Optional<OnlineFriendInBattleNode> friendInBattle,
            [JoinAll] SingleNode<InviteFriendsListComponent> inviteFriendsList, [JoinAll] SelectedBattle selectedBattle) {
            if (!friendInBattle.IsPresent() || friendInBattle.Get().battleGroup.Key != selectedBattle.Entity.Id) {
                CreateFriendListItem(friend.Entity.Id, inviteFriendsList.component);
            }
        }

        void CreateFriendListItem(long userId, InviteFriendsListComponent inviteFriendsList) {
            GameObject gameObject = Object.Instantiate(inviteFriendsList.FriendsListItem);
            gameObject.transform.SetParent(inviteFriendsList.transform, false);
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new UserGroupComponent(userId));
        }

        [OnEventFire]
        public void CreateFriendListItem(NodeAddedEvent e, OnlineFriendNode friend, [JoinByUser] Optional<SingleNode<InviteFriendListItemComponent>> listItem,
            [JoinAll] UserFriendsLoadedNode inviteFriendsList) {
            if (!listItem.IsPresent()) {
                CreateFriendListItem(friend.Entity.Id, inviteFriendsList.inviteFriendsList);
            }
        }

        [OnEventFire]
        public void RemoveFriendFromList(NodeRemoveEvent e, OnlineFriendNode onlineFriend, [JoinByUser] SingleNode<InviteFriendListItemComponent> listItem,
            [JoinAll] SingleNode<InviteFriendsListComponent> inviteFriendsList) {
            Object.Destroy(listItem.component.gameObject);
        }

        [OnEventComplete]
        public void RemoveFriendFromList(NodeAddedEvent e, OnlineFriendInBattleNode onlineFriend, [JoinByBattle] SelectedBattle battle, OnlineFriendInBattleNode onlineFriendToUi,
            [Context] [JoinByUser] SingleNode<InviteFriendListItemComponent> listItem) {
            Object.Destroy(listItem.component.gameObject);
        }

        [OnEventFire]
        public void AddFriendToList(NodeRemoveEvent e, FriendInBattleNode friendInBattle, [JoinSelf] OnlineFriendNode friend, [JoinByBattle] SelectedBattle battle,
            [JoinAll] SingleNode<InviteFriendsListComponent> inviteFriendsList) {
            CreateFriendListItem(friend.Entity.Id, inviteFriendsList.component);
        }

        [OnEventFire]
        public void SortListAndHideEmptyListNotification(NodeAddedEvent e, FriendUINode friendUI, [JoinByUser] FriendNode friend,
            [JoinAll] SingleNode<InviteFriendsListComponent> inviteFriendsList) {
            List<string> friendsUids = inviteFriendsList.component.FriendsUids;
            friendsUids.Add(friend.userUid.Uid);
            friendsUids.Sort(StringComparer.Ordinal);
            friendUI.inviteFriendListItem.transform.SetSiblingIndex(friendsUids.IndexOf(friend.userUid.Uid));
            inviteFriendsList.component.EmptyListNotification.SetActive(false);
        }

        [OnEventFire]
        public void ShowEmptyListNotification(NodeRemoveEvent e, SingleNode<InviteFriendListItemComponent> listItem, [JoinByUser] SingleNode<UserUidComponent> user,
            [JoinAll] SingleNode<InviteFriendsListComponent> inviteFriendsList) {
            List<string> friendsUids = inviteFriendsList.component.FriendsUids;
            friendsUids.Remove(user.component.Uid);

            if (friendsUids.Count == 0) {
                inviteFriendsList.component.EmptyListNotification.SetActive(true);
            }
        }

        [OnEventFire]
        public void LocalizeNotification(NodeAddedEvent e, FriendUINode friendUI, [JoinAll] SingleNode<InviteFriendsPanelLocalizationComponent> localization) {
            friendUI.inviteFriendListItem.NotificationText.text = localization.component.InviteSentNotification;
        }

        [OnEventFire]
        public void AddUserLabel(NodeAddedEvent e, FriendUINode friendUI) {
            GameObject userLabelInstance = UserLabelBuilder.CreateDefaultLabel();
            GameObject gameObject = new UserLabelBuilder(friendUI.userGroup.Key, userLabelInstance, null, false).SubscribeClick().Build();
            gameObject.transform.SetParent(friendUI.inviteFriendListItem.UserLabelContainer.transform, false);
        }

        [OnEventFire]
        public void ShowBattleLabel(NodeAddedEvent e, OnlineFriendInBattleNode onlineFriend, [Context] [JoinByUser] FriendUINode friendUI) {
            friendUI.inviteFriendListItem.BattleLabelContainer.SetActive(true);
        }

        [OnEventComplete]
        public void HideBattleLabel(NodeRemoveEvent e, OnlineFriendInBattleNode onlineFriend, [JoinByUser] FriendUINode friendUI) {
            friendUI.inviteFriendListItem.BattleLabelContainer.SetActive(false);
        }

        [OnEventFire]
        public void ShowInvitedFriendNotification(UserLabelClickEvent e, SingleNode<UserLabelComponent> userLabel, [JoinByUser] SingleNode<InviteFriendListItemComponent> friendUI,
            [JoinByUser] SingleNode<AcceptedFriendComponent> friend, [JoinAll] SelectedBattle battle, [JoinAll] SingleNode<InviteFriendsConfigComponent> inviteFriendsConfig) {
            friendUI.component.UserLabelContainer.SetActive(false);
            friendUI.component.NotificationContainer.SetActive(true);
            NewEvent<HideInvitedFriendNotificationEvent>().Attach(friend).ScheduleDelayed(inviteFriendsConfig.component.InviteSentNotificationDuration);
            ScheduleEvent(new InviteFriendToBattleEvent(battle.Entity.Id), friend);
        }

        [OnEventFire]
        public void HideInvitedFriendNotification(HideInvitedFriendNotificationEvent e, OnlineFriendNode friend, [JoinByUser] FriendUINode friendUI) {
            friendUI.inviteFriendListItem.NotificationContainer.SetActive(false);
            friendUI.inviteFriendListItem.UserLabelContainer.SetActive(true);
        }

        [OnEventFire]
        public void CleanList(NodeRemoveEvent e, SingleNode<InviteFriendsListComponent> inviteFriendsList) {
            inviteFriendsList.component.transform.DestroyChildren();
            inviteFriendsList.component.EmptyListNotification.SetActive(true);
        }

        public class FriendNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public UserAvatarComponent userAvatar;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class FriendInBattleNode : FriendNode {
            public BattleGroupComponent battleGroup;
        }

        public class OnlineFriendNode : FriendNode {
            public UserOnlineComponent userOnline;
        }

        public class OnlineFriendInBattleNode : OnlineFriendNode {
            public BattleGroupComponent battleGroup;
        }

        public class UserFriendsLoadedNode : Node {
            public InviteFriendsListComponent inviteFriendsList;

            public LoadUsersComponent loadUsers;

            public UsersLoadedComponent usersLoaded;
        }

        public class FriendUINode : Node {
            public InviteFriendListItemComponent inviteFriendListItem;

            public UserGroupComponent userGroup;
        }

        public class SelectedBattle : Node {
            public BattleComponent battle;

            public SelectedListItemComponent selectedListItem;
        }
    }
}