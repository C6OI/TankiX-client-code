using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientFriends.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsBattleShowSystem : ECSSystem {
        public class FriendUINode : Node {
            public FriendsListItemComponent friendsListItem;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class FriendInBattleNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public BattleGroupComponent battleGroup;

            public UserGroupComponent userGroup;
        }
    }
}