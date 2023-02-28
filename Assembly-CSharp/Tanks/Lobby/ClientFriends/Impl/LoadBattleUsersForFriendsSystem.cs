using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientFriends.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class LoadBattleUsersForFriendsSystem : ECSSystem {
        public class FriendInBattleNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public BattleGroupComponent battleGroup;

            public UserGroupComponent userGroup;
        }
    }
}