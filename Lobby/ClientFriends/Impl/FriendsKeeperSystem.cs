using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientFriends.Impl {
    public class FriendsKeeperSystem : ECSSystem {
        [OnEventFire]
        public void LoadedFriendsId(FriendsLoadedEvent e, SingleNode<ClientSessionComponent> session,
            [JoinByUser] UserNode user) {
            FriendsComponent friendsComponent = new();
            friendsComponent.AcceptedFriendsIds = e.AcceptedFriendsIds;
            friendsComponent.IncommingFriendsIds = e.IncommingFriendsIds;
            friendsComponent.OutgoingFriendsIds = e.OutgoingFriendsIds;
            user.Entity.AddComponent(friendsComponent);
        }

        [OnEventFire]
        public void AddAcceptedFriend(AcceptedFriendAddedEvent e, SingleNode<FriendsComponent> userFriends) {
            userFriends.component.AcceptedFriendsIds.Add(e.FriendId);
            MarkUserIfEntityLoaded<AcceptedFriendComponent>(e.FriendId);
        }

        [OnEventFire]
        public void RemoveAcceptedFriend(AcceptedFriendRemovedEvent e, SingleNode<FriendsComponent> userFriends) {
            userFriends.component.AcceptedFriendsIds.Remove(e.FriendId);
            UnMarkUserIfEntityLoaded<AcceptedFriendComponent>(e.FriendId);
        }

        [OnEventFire]
        public void AddIncommingFriend(IncomingFriendAddedEvent e, SingleNode<FriendsComponent> userFriends) {
            userFriends.component.IncommingFriendsIds.Add(e.FriendId);
            MarkUserIfEntityLoaded<IncommingFriendComponent>(e.FriendId);
        }

        [OnEventFire]
        public void RemoveIncommingFriend(IncomingFriendRemovedEvent e, SingleNode<FriendsComponent> userFriends) {
            userFriends.component.IncommingFriendsIds.Remove(e.FriendId);
            UnMarkUserIfEntityLoaded<IncommingFriendComponent>(e.FriendId);
        }

        [OnEventFire]
        public void AddOutgoingFriend(OutgoingFriendAddedEvent e, SingleNode<FriendsComponent> userFriends) {
            userFriends.component.OutgoingFriendsIds.Add(e.FriendId);
            MarkUserIfEntityLoaded<OutgoingFriendComponent>(e.FriendId);
        }

        [OnEventFire]
        public void RemoveOutgoingFriend(OutgoingFriendRemovedEvent e, SingleNode<FriendsComponent> userFriends) {
            userFriends.component.OutgoingFriendsIds.Remove(e.FriendId);
            UnMarkUserIfEntityLoaded<OutgoingFriendComponent>(e.FriendId);
        }

        void MarkUserIfEntityLoaded<T>(long entityId) where T : Component, new() {
            if (Flow.Current.EntityRegistry.ContainsEntity(entityId)) {
                MarkUser<T>(Flow.Current.EntityRegistry.GetEntity(entityId));
            }
        }

        void MarkUser<T>(Entity user) where T : Component, new() {
            user.AddComponent<T>();
            user.AddComponent<FriendComponent>();
        }

        void UnMarkUserIfEntityLoaded<T>(long entityId) where T : Component {
            if (Flow.Current.EntityRegistry.ContainsEntity(entityId)) {
                UnMarkUser<T>(Flow.Current.EntityRegistry.GetEntity(entityId));
            }
        }

        void UnMarkUser<T>(Entity user) where T : Component {
            user.RemoveComponent<T>();
            user.RemoveComponent<FriendComponent>();
        }

        [OnEventFire]
        public void MarkLoadedUser(NodeAddedEvent e, SingleNode<UserComponent> user,
            [JoinAll] SingleNode<FriendsComponent> userFriends) {
            if (userFriends.component.AcceptedFriendsIds.Contains(user.Entity.Id)) {
                MarkUser<AcceptedFriendComponent>(user.Entity);
            } else if (userFriends.component.IncommingFriendsIds.Contains(user.Entity.Id)) {
                MarkUser<IncommingFriendComponent>(user.Entity);
            } else if (userFriends.component.OutgoingFriendsIds.Contains(user.Entity.Id)) {
                MarkUser<OutgoingFriendComponent>(user.Entity);
            }
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }
    }
}