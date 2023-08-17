using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientFriends.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class LoadBattleUsersForFriendsSystem : ECSSystem {
        [OnEventFire]
        public void LoadBattleUser(NodeAddedEvent e, FriendInBattleNode friendInBattleNode,
            [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<FriendsScreenComponent> screen) =>
            ScheduleEvent(new RequestLoadBattleUserForLabelEvent(session.Entity), friendInBattleNode);

        [OnEventFire]
        public void LoadBattleUsers(NodeAddedEvent e, SingleNode<FriendsScreenComponent> screen,
            [JoinAll] SingleNode<ClientSessionComponent> session,
            [Combine] [JoinAll] FriendInBattleNode friendInBattleNode) =>
            ScheduleEvent(new RequestLoadBattleUserForLabelEvent(session.Entity), friendInBattleNode);

        public class FriendInBattleNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public BattleGroupComponent battleGroup;

            public UserGroupComponent userGroup;
        }
    }
}