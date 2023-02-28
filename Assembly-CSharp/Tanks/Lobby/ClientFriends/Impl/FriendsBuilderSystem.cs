using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsBuilderSystem : ECSSystem {
        [OnEventFire]
        public void RequestSortedFriends(NodeAddedEvent e, SingleNode<FriendsListUIComponent> friendsScreen, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent<LoadSortedFriendsIdsEvent>(session);
        }

        [OnEventFire]
        public void CreateUIsForFriends(SortedFriendsIdsLoadedEvent e, SingleNode<ClientSessionComponent> session, [JoinByUser] SingleNode<FriendsComponent> friends,
            [JoinAll] SingleNode<FriendsListUIComponent> friendsScreen) {
            friendsScreen.component.AddFriends(e.friendsOutgoingIds, FriendType.Outgoing);
            friendsScreen.component.AddFriends(e.friendsAcceptedIds, FriendType.Accepted);
            friendsScreen.component.AddFriends(e.friendsIncomingIds, FriendType.Incoming);
        }

        [OnEventFire]
        public void RemoveOutdatedUI(NodeAddedEvent e, FriendUI newFriendUI, [Combine] [JoinByUser] FriendUI oldFriendUI) {
            if (newFriendUI.Entity.Id != oldFriendUI.Entity.Id) {
                Object.Destroy(oldFriendUI.friendsListItem.gameObject);
            }
        }

        [OnEventFire]
        public void RemoveFriendUI(NodeRemoveEvent e, SingleNode<FriendComponent> friend, [JoinByUser] SingleNode<FriendsListItemComponent> friendUI) {
            Object.Destroy(friendUI.component.gameObject);
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