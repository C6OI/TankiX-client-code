using System;
using System.Collections;
using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientFriends.Impl;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteToSquadSystem : ECSSystem {
        [OnEventFire]
        public void Invite(ButtonClickEvent e, InviteToSquadButtonNode button, [JoinByUser] FriendUserNode friend, [JoinAll] SingleNode<FriendsComponent> friends,
            [JoinAll] SelfUserNode selfUser, [JoinBySquad] Optional<SquadNode> squad) {
            ScheduleEvent(new FriendInviteToSquadEvent(friend.Entity.Id, InteractionSource.SQUAD, !squad.IsPresent() ? 0 : squad.Get().Entity.Id), friend);
        }

        [OnEventFire]
        public void InviteToSquad(FriendInviteToSquadEvent e, FriendUserNode friend, [JoinAll] SelfUserNode selfUser, [JoinBySquad] Optional<SquadNode> squad,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<FriendsComponent> friends) {
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            if (friend.userRank.Rank < 4) {
                CantInviteFriendIntoSquadDialogComponent cantInviteFriendIntoSquadDialogComponent = dialogs.component.Get<CantInviteFriendIntoSquadDialogComponent>();
                cantInviteFriendIntoSquadDialogComponent.Show(friend.userUid.Uid, animators);
                return;
            }

            if (friend.Entity.HasComponent<MatchMakingUserComponent>()) {
                CantInviteFriendIntoSquadDialogComponent cantInviteFriendIntoSquadDialogComponent2 = dialogs.component.Get<CantInviteFriendIntoSquadDialogComponent>();
                cantInviteFriendIntoSquadDialogComponent2.Show(friend.userUid.Uid, animators);
                return;
            }

            if (friend.Entity.HasComponent<InvitedToSquadUserComponent>()) {
                friend.Entity.RemoveComponent<InvitedToSquadUserComponent>();
            }

            friends.component.InSquadInvitations[friend.userGroup.Key] = DateTime.Now;
            friend.Entity.AddComponent<InvitedToSquadUserComponent>();
            Invite(friend, squad, selfUser);
        }

        void Invite(FriendUserNode friend, Optional<SquadNode> squad, SelfUserNode selfUser) {
            NewEvent(new InviteToSquadEvent {
                InvitedUsersIds = new long[1] { friend.Entity.Id }
            }).Attach(selfUser).Schedule();
        }

        [OnEventFire]
        public void RequestToSquad(RequestToSquadInternalEvent e, UserInSquadNode friend, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            NewEvent(new RequestToSquadEvent {
                ToUserId = friend.userGroup.Key,
                SquadId = friend.squadGroup.Key
            }).Attach(selfUser).Schedule();
        }

        [OnEventComplete]
        public void UserInSquad(NodeAddedEvent e, InviteToSquadUserLabelNode label, [JoinByUser] [Context] UserInSquadNode squadUser) {
            label.waitingForInviteToSquadAnswerUi.AlreadyInSquad = true;
        }

        [OnEventFire]
        public void UserLeaveSquad(NodeRemoveEvent e, UserInSquadNode squadUser, [JoinByUser] InviteToSquadUserLabelNode label) {
            label.waitingForInviteToSquadAnswerUi.AlreadyInSquad = false;
        }

        [OnEventFire]
        public void ShowInviteDialog(InvitedToSquadEvent e, NotBattleUser selfUser, [JoinAll] DialogsNode dialogs,
            [JoinAll] Optional<SingleNode<EulaNotificationComponent>> eulaNotification, [JoinAll] Optional<SingleNode<PrivacyPolicyNotificationComponent>> ppNotification) {
            if (!eulaNotification.IsPresent() && !ppNotification.IsPresent()) {
                Debug.Log(string.Concat("InviteToLobbySystem.ShowInviteDialog ", selfUser.Entity, " UserUid=", e.UserUid, " FromUserId=", e.FromUserId, " EngineId=", e.EngineId));
                ShowInviteDialog(selfUser, e.EngineId, e.UserUid, e.FromUserId, dialogs);
            }
        }

        [OnEventFire]
        public void ShowRequestDialog(RequestedToSquadEvent e, NotBattleUser selfUser, [JoinAll] DialogsNode dialogs,
            [JoinAll] Optional<SingleNode<EulaNotificationComponent>> eulaNotification, [JoinAll] Optional<SingleNode<PrivacyPolicyNotificationComponent>> ppNotification) {
            if (!eulaNotification.IsPresent() && !ppNotification.IsPresent()) {
                Debug.Log(string.Concat("InviteToLobbySystem.ShowRequestDialog ", selfUser.Entity, " UserUid=", e.UserUid, " FromUserId=", e.FromUserId, " EngineId=", e.EngineId,
                    " SquadId=", e.SquadId));

                ShowRequestDialog(selfUser, e.EngineId, e.UserUid, e.FromUserId, e.SquadId, dialogs);
            }
        }

        [OnEventFire]
        public void CloseSquadInviteDialog(NodeAddedEvent e, LobbyUserNode selfUser, [JoinAll] [Combine] SingleNode<InviteToSquadDialogComponent> inviteToSquadDialog) {
            inviteToSquadDialog.Entity.GetComponent<InviteDialogComponent>().Hide();
        }

        void ShowInviteDialog(SelfUserNode user, long engineId, string userUid, long fromUserId, DialogsNode dialogs) {
            InviteToSquadDialogComponent inviteToSquadDialogComponent = dialogs.dialogs60.Get<InviteToSquadDialogComponent>();

            if (!CanShowInviteWindow(inviteToSquadDialogComponent)) {
                ScheduleEvent(new RejectInviteToSquadEvent {
                    FromUserId = fromUserId,
                    EngineId = engineId
                }, user);
            } else {
                InviteToSquadDialogComponent component = dialogs.dialogs60.Get<NotificationsStackContainerComponent>().CreateNotification(inviteToSquadDialogComponent.gameObject)
                    .GetComponent<InviteToSquadDialogComponent>();

                component.FromUserId = fromUserId;
                component.EngineId = engineId;
                component.Show(userUid, false, true);
            }
        }

        void ShowRequestDialog(SelfUserNode user, long engineId, string userUid, long fromUserId, long squadId, DialogsNode dialogs) {
            InviteToSquadDialogComponent inviteToSquadDialogComponent = dialogs.dialogs60.Get<InviteToSquadDialogComponent>();

            if (!CanShowInviteWindow(inviteToSquadDialogComponent)) {
                ScheduleEvent(new RejectRequestToSquadEvent {
                    FromUserId = fromUserId,
                    SquadId = squadId,
                    SquadEngineId = engineId
                }, user);
            } else {
                InviteToSquadDialogComponent component = dialogs.dialogs60.Get<NotificationsStackContainerComponent>().CreateNotification(inviteToSquadDialogComponent.gameObject)
                    .GetComponent<InviteToSquadDialogComponent>();

                component.FromUserId = fromUserId;
                component.EngineId = engineId;
                component.SquadId = squadId;
                component.Show(userUid, false, false);
            }
        }

        [OnEventFire]
        public void HideInviteDialogOnEula(NodeAddedEvent e, SingleNode<EulaNotificationComponent> eulaNotification, [Combine] SingleNode<InviteToSquadDialogComponent> inviteDialog) {
            inviteDialog.component.Hide();
        }

        [OnEventFire]
        public void HideInviteDialogOnPP(NodeAddedEvent e, SingleNode<PrivacyPolicyNotificationComponent> ppNotification,
            [Combine] SingleNode<InviteToSquadDialogComponent> inviteDialog) {
            inviteDialog.component.Hide();
        }

        bool CanShowInviteWindow(InviteToSquadDialogComponent window) {
            if (window == null || MainScreenComponent.Instance == null) {
                return false;
            }

            if (window.gameObject.activeSelf) {
                return false;
            }

            return true;
        }

        [OnEventFire]
        public void OnDialogConfirm(DialogConfirmEvent e, SingleNode<InviteToSquadDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user,
            [JoinByBattleLobby] Optional<SingleNode<BattleLobbyComponent>> lobby) {
            if (lobby.IsPresent() && !lobby.Get().Entity.HasComponent<CustomBattleLobbyComponent>()) {
                return;
            }

            bool flag = lobby.IsPresent() && lobby.Get().Entity.HasComponent<CustomBattleLobbyComponent>();

            if (dialog.component.invite) {
                if (flag) {
                    ScheduleEvent<ClientExitLobbyEvent>(lobby.Get());
                } else {
                    ScheduleEvent<CancelMatchSearchingEvent>(user);
                }

                NewEvent(new AcceptInviteToSquadEvent {
                    EngineId = dialog.component.EngineId,
                    FromUserId = dialog.component.FromUserId
                }).Attach(user).Schedule();
            } else {
                ScheduleEvent(new AcceptRequestToSquadEvent {
                    FromUserId = dialog.component.FromUserId,
                    SquadId = dialog.component.SquadId,
                    SquadEngineId = dialog.component.EngineId
                }, user);
            }
        }

        [OnEventFire]
        public void OnDialogDecline(DialogDeclineEvent e, SingleNode<InviteToSquadDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user) {
            Debug.Log(string.Concat("InviteToLobbySystem.OnDialogDecline ", user.Entity, " ", dialog.component.invite, " ", dialog.component.FromUserId, " ", dialog.component.EngineId));

            if (dialog.component.invite) {
                ScheduleEvent(new RejectInviteToSquadEvent {
                    FromUserId = dialog.component.FromUserId,
                    EngineId = dialog.component.EngineId
                }, user);
            } else {
                ScheduleEvent(new RejectRequestToSquadEvent {
                    FromUserId = dialog.component.FromUserId,
                    SquadId = dialog.component.SquadId,
                    SquadEngineId = dialog.component.EngineId
                }, user);
            }
        }

        [OnEventFire]
        public void RequestToSquadCanceled(RequestToSquadCanceledEvent e, SingleNode<SelfUserComponent> user) {
            Debug.Log("InviteToLobbySystem.RequestToSquadCanceled");
        }

        [OnEventFire]
        public void RequestToSquadRejected(RequestToSquadRejectedEvent e, SingleNode<SelfUserComponent> user, [JoinAll] ICollection<FriendTooltipContentNode> friendTooltips) {
            Debug.Log("InviteToLobbySystem.RequestToSquadRejected " + e.Reason);

            foreach (FriendTooltipContentNode friendTooltip in friendTooltips) {
                if (friendTooltip.userGroup.Key == e.RequestReceiverId && e.Reason == RejectRequestToSquadReason.SQUAD_IS_FULL) {
                    friendTooltip.friendInteractionTooltipContent.SquadIsFull();
                }
            }
        }

        [OnEventFire]
        public void InviteToSquadCanceled(InviteToSquadCanceledEvent e, SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<NotificationsStackContainerComponent> container) {
            Debug.Log("InviteToLobbySystem.InviteToSquadCanceled");
            IEnumerator enumerator = container.component.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    InviteToSquadDialogComponent component = transform.GetComponent<InviteToSquadDialogComponent>();

                    if (component != null && component.invite) {
                        component.GetComponent<InviteDialogComponent>().Hide();
                    }
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        [OnEventFire]
        public void InviteToSquadRejected(InviteToSquadRejectedEvent e, SingleNode<SelfUserComponent> user) {
            Debug.Log("InviteToLobbySystem.InviteToSquadRejected");
        }

        void DelayUpdateInviteToSquadButton(Node user) {
            NewEvent<UpdateUserInviteToSquadButtonEvent>().Attach(user).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateInviteToSquadButton(UpdateUserInviteToSquadButtonEvent e, UserNode user, [JoinByUser] [Combine] UserLabelStateNode userLabel,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            bool flag = user.Entity.HasComponent<SquadGroupComponent>();
            bool flag2 = user.Entity.HasComponent<UserOnlineComponent>();
            bool flag3 = user.Entity.HasComponent<MatchMakingUserComponent>();
            bool flag4 = user.Entity.HasComponent<BattleGroupComponent>();
            bool flag5 = selfUser.Entity.HasComponent<MatchMakingUserComponent>();

            if (userLabel.userLabelState.DisableInviteOnlyForSquadState) {
                userLabel.userLabelState.CanBeInvited = !flag || !flag3 && !flag4;
            } else {
                userLabel.userLabelState.CanBeInvited = flag2 && !flag3 && !flag4 && !flag5;
            }
        }

        [OnEventFire]
        public void UserOnline(NodeAddedEvent e, UserOnlineNode user, [Context] [JoinByUser] [Combine] UserLabelStateNode userLabel) {
            DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserOffline(NodeRemoveEvent e, UserOnlineNode user, [JoinByUser] [Combine] UserLabelStateNode userLabel) {
            DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserInMatchMaking(NodeAddedEvent e, UserInMatchMakingNode user, [JoinByUser] [Context] [Combine] UserLabelStateNode userLabel) {
            DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserOutMatchMaking(NodeRemoveEvent e, UserInMatchMakingNode user, [JoinByUser] [Combine] UserLabelStateNode userLabel) {
            DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserInBattle(NodeAddedEvent e, UserInBattleNode user, [JoinByUser] [Context] [Combine] UserLabelStateNode userLabel) {
            DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserOutBattle(NodeRemoveEvent e, UserInBattleNode user, [JoinByUser] [Combine] UserLabelStateNode userLabel) {
            DelayUpdateInviteToSquadButton(user);
        }

        public class SquadNode : Node {
            public SquadComponent squad;

            public SquadConfigComponent squadConfig;

            public SquadGroupComponent squadGroup;
        }

        public class UserInSquadNode : Node {
            public SquadGroupComponent squadGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;
        }

        public class SelfUserInSquadNode : SelfUserNode {
            public SquadGroupComponent squadGroup;
        }

        [Not(typeof(BattleGroupComponent))]
        public class NotBattleUser : SelfUserNode { }

        public class LobbyUserNode : SelfUserNode {
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class FriendUserNode : Node {
            public AcceptedFriendComponent acceptedFriend;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class DialogsNode : Node {
            public Dialogs60Component dialogs60;
        }

        public class InviteToSquadUserLabelNode : Node {
            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public WaitingForInviteToSquadAnswerUIComponent waitingForInviteToSquadAnswerUi;
        }

        public class InviteToSquadButtonNode : Node {
            public SendInviteToSquadButtonComponent sendInviteToSquadButton;
            public UserGroupComponent userGroup;
        }

        public class UserInMatchMakingNode : Node {
            public MatchMakingUserComponent matchMakingUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class FriendInBattleNode : Node {
            public BattleGroupComponent battleGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class FriendTooltipContentNode : Node {
            public FriendInteractionTooltipContentComponent friendInteractionTooltipContent;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class UserOnlineNode : UserNode {
            public UserOnlineComponent userOnline;
        }

        public class UserLabelStateNode : Node {
            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public UserLabelStateComponent userLabelState;
        }

        public class UserInBattleNode : UserNode {
            public BattleGroupComponent battleGroup;
        }

        public class UpdateUserInviteToSquadButtonEvent : Event { }
    }
}