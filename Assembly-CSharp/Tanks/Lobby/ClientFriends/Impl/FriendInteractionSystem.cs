using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendInteractionSystem : ECSSystem {
        [OnEventFire]
        public void ShowTooltipInLobby(RightMouseButtonClickEvent e, FriendLabelNode userButton, [JoinByUser] FriendNode friend, [JoinByUser] Optional<UserInBattleNode> userInBattle,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            bool flag = friend.Entity.HasComponent<AcceptedFriendComponent>();
            bool flag2 = selfUser.Entity.HasComponent<UserAdminComponent>();
            CheckForSpectatorButtonShowEvent checkForSpectatorButtonShowEvent = new();
            ScheduleEvent(checkForSpectatorButtonShowEvent, friend);
            CheckForShowInviteToSquadEvent checkForShowInviteToSquadEvent = new();
            ScheduleEvent(checkForShowInviteToSquadEvent, friend);
            FriendInteractionTooltipData friendInteractionTooltipData = new();
            friendInteractionTooltipData.FriendEntity = friend.Entity;
            friendInteractionTooltipData.ShowRemoveButton = flag;
            friendInteractionTooltipData.ShowEnterAsSpectatorButton = userInBattle.IsPresent() && (flag || flag2) && checkForSpectatorButtonShowEvent.CanGoToSpectatorMode;
            friendInteractionTooltipData.ShowInviteToSquadButton = checkForShowInviteToSquadEvent.ShowInviteToSquadButton;
            friendInteractionTooltipData.ActiveShowInviteToSquadButton = checkForShowInviteToSquadEvent.ActiveInviteToSquadButton;
            friendInteractionTooltipData.ShowRequestToSquadButton = checkForShowInviteToSquadEvent.ShowRequestToInviteToSquadButton;
            friendInteractionTooltipData.ShowChatButton = friend.Entity.HasComponent<UserOnlineComponent>();
            FriendInteractionTooltipData data = friendInteractionTooltipData;
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, userButton.friendInteractionButton.tooltipPrefab, false);
        }

        [OnEventFire]
        public void RemoveFriend(RemoveFriendButtonEvent e, AcceptedFriendNode friend, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            ScheduleEvent(new BreakOffFriendEvent(friend.Entity), selfUser);
            friendsScreen.component.RemoveUser(friend.Entity.Id, true);
        }

        public class FriendLabelNode : Node {
            public FriendInteractionButtonComponent friendInteractionButton;

            public IncomingFriendButtonsComponent incomingFriendButtons;

            public OutgoingFriendButtonsComponent outgoingFriendButtons;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;
        }

        [Not(typeof(SelfUserComponent))]
        public class FriendNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class AcceptedFriendNode : FriendNode {
            public AcceptedFriendComponent acceptedFriend;
        }

        public class UserInBattleNode : FriendNode {
            public BattleGroupComponent battleGroup;
        }
    }
}