using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientFriends.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsBattleShowSystem : ECSSystem {
        [OnEventFire]
        public void ShowBattle(NodeAddedEvent e, FriendUINode friendUINode,
            [Context] [JoinByUser] FriendInBattleNode friendNode) {
            BattleLabelBuilder battleLabelBuilder = new(friendNode.battleGroup.Key);
            GameObject gameObject = battleLabelBuilder.Build();
            gameObject.transform.SetParent(friendUINode.friendsListItem.BattleLabelContainer.gameObject.transform, false);
        }

        [OnEventFire]
        public void HideBattle(NodeRemoveEvent e, FriendInBattleNode friend, [JoinByUser] FriendUINode friendUINode) =>
            Object.Destroy(friendUINode.friendsListItem.BattleLabelContainer.GetComponentInChildren<BattleLabelComponent>()
                .gameObject);

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