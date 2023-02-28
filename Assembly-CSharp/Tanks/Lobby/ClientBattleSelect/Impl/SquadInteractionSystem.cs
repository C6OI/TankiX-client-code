using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadInteractionSystem : ECSSystem {
        [OnEventFire]
        public void ShowSelfUserTooltip(RightMouseButtonClickEvent e, SelfUserLabelNode selfUserButton, [JoinAll] SelfUserInSquadNode selfUser,
            [JoinAll] ICollection<TeammateNode> teammates, [JoinAll] SingleNode<FriendsComponent> friends) {
            if (teammates.Count > 0) {
                ShowTooltip(selfUser, selfUser, selfUserButton.squadTeammateInteractionButton, friends.component);
            }
        }

        [OnEventFire]
        public void ShowTooltipInLobby(RightMouseButtonClickEvent e, TeammateLabelNode userButton, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser,
            [JoinAll] SingleNode<FriendsComponent> friends) {
            ShowTooltip(user, selfUser, userButton.squadTeammateInteractionButton, friends.component);
        }

        public void ShowTooltip(UserNode user, SelfUserNode selfUser, SquadTeammateInteractionButtonComponent squadTeammateInteractionButton, FriendsComponent friends) {
            bool flag = user.Entity.HasComponent<SelfUserComponent>();
            bool flag2 = selfUser.Entity.HasComponent<SquadLeaderComponent>();
            bool flag3 = user.Entity.HasComponent<AcceptedFriendComponent>();
            bool flag4 = selfUser.Entity.HasComponent<MatchMakingUserComponent>();
            bool flag5 = friends.OutgoingFriendsIds.Contains(user.Entity.Id);
            SquadTeammateInteractionTooltipContentData data;

            if (flag) {
                if (flag4) {
                    return;
                }

                SquadTeammateInteractionTooltipContentData squadTeammateInteractionTooltipContentData = new();
                squadTeammateInteractionTooltipContentData.teammateEntity = user.Entity;
                squadTeammateInteractionTooltipContentData.ShowLeaveSquadButton = true;
                data = squadTeammateInteractionTooltipContentData;
            } else {
                SquadTeammateInteractionTooltipContentData squadTeammateInteractionTooltipContentData = new();
                squadTeammateInteractionTooltipContentData.teammateEntity = user.Entity;
                squadTeammateInteractionTooltipContentData.ShowProfileButton = true;
                squadTeammateInteractionTooltipContentData.ShowLeaveSquadButton = false;
                squadTeammateInteractionTooltipContentData.ShowRemoveFromSquadButton = !flag4;
                squadTeammateInteractionTooltipContentData.ActiveRemoveFromSquadButton = flag2;
                squadTeammateInteractionTooltipContentData.ShowGiveLeaderButton = !flag4;
                squadTeammateInteractionTooltipContentData.ActiveGiveLeaderButton = flag2;
                squadTeammateInteractionTooltipContentData.ShowAddFriendButton = !flag3 && !flag5;
                squadTeammateInteractionTooltipContentData.ShowFriendRequestSentButton = !flag3 && flag5;
                data = squadTeammateInteractionTooltipContentData;
            }

            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, squadTeammateInteractionButton.tooltipPrefab, false);
        }

        [OnEventFire]
        public void ChangeSquadLeader(ChangeSquadLeaderInternalEvent e, UserNode teammate, [JoinAll] SelfSquadLeaderNode selfSquadLeader) {
            ScheduleEvent(new ChangeSquadLeaderEvent {
                NewLeaderUserId = teammate.Entity.Id
            }, selfSquadLeader);
        }

        [OnEventFire]
        public void KickOutFromSquad(KickOutFromSquadInternalEvent e, UserNode teammate, [JoinAll] SelfSquadLeaderNode selfSquadLeader) {
            ScheduleEvent(new KickOutFromSquadEvent {
                KickedOutUserId = teammate.Entity.Id
            }, selfSquadLeader);
        }

        [OnEventFire]
        public void LeaveSquad(LeaveSquadInternalEvent e, UserNode teammate) {
            ScheduleEvent<LeaveSquadEvent>(teammate);
        }

        [OnEventFire]
        public void RequestFriend(RequestFriendSquadInternalEvent e, UserNode user, [JoinBySquad] SingleNode<SquadComponent> squad, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            ScheduleEvent(new RequestFriendshipByUserId(user.Entity.Id, InteractionSource.SQUAD, squad.Entity.Id), selfUser);
        }

        public class TeammateLabelNode : Node {
            public SquadTeammateInteractionButtonComponent squadTeammateInteractionButton;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;
        }

        [Not(typeof(UserLabelComponent))]
        public class SelfUserLabelNode : Node {
            public SquadTeammateInteractionButtonComponent squadTeammateInteractionButton;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : UserNode {
            public SelfUserComponent selfUser;
        }

        public class SelfUserInSquadNode : SelfUserNode {
            public SquadGroupComponent squadGroup;
        }

        public class SelfSquadLeaderNode : SelfUserNode {
            public SquadLeaderComponent squadLeader;
        }

        [Not(typeof(SelfUserComponent))]
        public class TeammateNode : Node {
            public SquadGroupComponent squadGroup;
            public UserComponent user;
        }
    }
}