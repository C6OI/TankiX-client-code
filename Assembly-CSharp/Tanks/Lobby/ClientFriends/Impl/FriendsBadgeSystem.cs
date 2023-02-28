using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientFriends.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsBadgeSystem : ECSSystem {
        [OnEventFire]
        public void AddCounter(NodeAddedEvent e, SingleNode<FriendsComponent> friends) {
            friends.Entity.AddComponent(new FriendsBadgeCounterComponent {
                Counter = friends.component.IncommingFriendsIds.Count
            });
        }

        [OnEventFire]
        public void AddBadgeOnEnter(NodeAddedEvent e, SingleNode<OpenFriendsScreenButtonComponent> button, SingleNode<FriendsBadgeCounterComponent> friends) {
            Update(button, friends);
        }

        void Update(SingleNode<OpenFriendsScreenButtonComponent> button, SingleNode<FriendsBadgeCounterComponent> friends) {
            if (friends.component.Counter <= 0) {
                Log.Info("HideBadge");
                friends.component.Counter = Mathf.Max(0, friends.component.Counter);
                button.component.countingBadge.SetActive(false);
            } else {
                Log.Info("ShowBadge");
                button.component.countingBadge.Count = friends.component.Counter;
                button.component.countingBadge.SetActive(true);
            }
        }

        [OnEventFire]
        public void HideBadgeOnEnterScreen(ButtonClickEvent e, SingleNode<OpenFriendsScreenButtonComponent> button, [JoinAll] SingleNode<FriendsBadgeCounterComponent> friends) {
            friends.component.Counter = 0;
            Update(button, friends);
        }

        [OnEventFire]
        public void AddAcceptedFriend(AcceptedFriendAddedEvent e, SingleNode<FriendsBadgeCounterComponent> friends,
            [JoinAll] Optional<SingleNode<OpenFriendsScreenButtonComponent>> button) {
            friends.component.Counter--;
            UpdateBadgeIfPresent(friends, button);
        }

        [OnEventFire]
        public void AddIncommingFriend(IncomingFriendAddedEvent e, SingleNode<FriendsBadgeCounterComponent> friends,
            [JoinAll] Optional<SingleNode<OpenFriendsScreenButtonComponent>> button) {
            friends.component.Counter++;
            UpdateBadgeIfPresent(friends, button);
        }

        void UpdateBadgeIfPresent(SingleNode<FriendsBadgeCounterComponent> friends, Optional<SingleNode<OpenFriendsScreenButtonComponent>> button) {
            if (button.IsPresent()) {
                Update(button.Get(), friends);
            }
        }

        [OnEventFire]
        public void SetCountOnEnter(NodeAddedEvent e, SingleNode<IncomingFriendsCounterComponent> counter, [JoinAll] SingleNode<FriendsComponent> friends) {
            counter.component.Count = friends.component.IncommingFriendsIds.Count;
        }

        [OnEventFire]
        public void AddAcceptedFriend(AcceptedFriendAddedEvent e, Node any, [JoinAll] SingleNode<IncomingFriendsCounterComponent> counter) {
            counter.component.Count--;
        }

        [OnEventFire]
        public void RejectFriend(RejectFriendEvent e, Node any, [JoinAll] SingleNode<IncomingFriendsCounterComponent> counter) {
            counter.component.Count--;
        }

        [OnEventFire]
        public void RejectAll(RejectAllFriendsEvent e, Node any, [JoinAll] SingleNode<IncomingFriendsCounterComponent> counter) {
            counter.component.Count = 0;
        }
    }
}