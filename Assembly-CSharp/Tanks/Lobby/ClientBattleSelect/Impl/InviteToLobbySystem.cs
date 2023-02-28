using System;
using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteToLobbySystem : ECSSystem {
        [OnEventFire]
        public void Invite(ButtonClickEvent e, InviteToLobbyButtonNode button, [JoinByUser] UserNode user, [JoinAll] LobbyNode lobby, [JoinAll] SingleNode<FriendsComponent> friends) {
            Invite(new List<long> { user.Entity.Id }, lobby);

            if (user.Entity.HasComponent<InvitedToLobbyUserComponent>()) {
                user.Entity.RemoveComponent<InvitedToLobbyUserComponent>();
            }

            friends.component.InLobbyInvitations[user.userGroup.Key] = DateTime.Now;
            user.Entity.AddComponent<InvitedToLobbyUserComponent>();
        }

        [OnEventComplete]
        public void UserInSameLobby(NodeAddedEvent e, WaitingInviteAnswerUserLabelNode label, [JoinByUser] LobbyUserNode lobbyUser, [JoinByBattleLobby] SelfLobbyUserNode selfLobbyUser) {
            label.waitingForInviteToLobbyAnswerUi.AlreadyInLobby = true;
        }

        [OnEventComplete]
        public void UserInSameLobby(NodeAddedEvent e, LobbyUserNode lobbyUser, [JoinByUser] WaitingInviteAnswerUserLabelNode label, LobbyUserNode lobbyUser1,
            [JoinByBattleLobby] SelfLobbyUserNode selfLobbyUser) {
            label.waitingForInviteToLobbyAnswerUi.AlreadyInLobby = true;
        }

        [OnEventFire]
        public void UserLeaveLobby(NodeRemoveEvent e, LobbyUserNode lobbyUser, [JoinByUser] WaitingInviteAnswerUserLabelNode label) {
            label.waitingForInviteToLobbyAnswerUi.AlreadyInLobby = false;
        }

        void Invite(List<long> userIds, LobbyNode lobby) {
            ScheduleEvent(new InviteToLobbyEvent {
                InvitedUserIds = userIds.ToArray()
            }, lobby);
        }

        [OnEventFire]
        public void ShowInviteDialog(InvitedToLobbyEvent e, SingleNode<SelfUserComponent> user, [JoinAll] DialogsNode dialogs, [JoinAll] Optional<LobbyNode> lobby,
            [JoinAll] Optional<SingleNode<SelfBattleUserComponent>> selfBattleUser, [JoinAll] Optional<SingleNode<EulaNotificationComponent>> eulaNotification,
            [JoinAll] Optional<SingleNode<PrivacyPolicyNotificationComponent>> ppNotification) {
            InviteToLobbyDialogComponent inviteToLobbyDialogComponent = dialogs.dialogs60.Get<InviteToLobbyDialogComponent>();

            if (!(inviteToLobbyDialogComponent == null) && !(MainScreenComponent.Instance == null) && !inviteToLobbyDialogComponent.gameObject.activeSelf &&
                (!lobby.IsPresent() || lobby.Get().Entity.Id != e.lobbyId) && !eulaNotification.IsPresent() && !ppNotification.IsPresent()) {
                inviteToLobbyDialogComponent.engineId = e.engineId;
                inviteToLobbyDialogComponent.lobbyId = e.lobbyId;

                string text = !user.Entity.HasComponent<SquadGroupComponent>() ? inviteToLobbyDialogComponent.messageForSingleUser
                                  : (string)(!user.Entity.HasComponent<SquadLeaderComponent>() ? inviteToLobbyDialogComponent.messageForSquadMember
                                                 : inviteToLobbyDialogComponent.messageForSquadLeader);

                text = text.Replace("{0}", e.userUid);

                InviteToLobbyDialogComponent component = dialogs.dialogs60.Get<NotificationsStackContainerComponent>().CreateNotification(inviteToLobbyDialogComponent.gameObject)
                    .GetComponent<InviteToLobbyDialogComponent>();

                component.GetComponent<InviteDialogComponent>().Show(text, selfBattleUser.IsPresent());
            }
        }

        [OnEventFire]
        public void HideInviteDialogOnEula(NodeAddedEvent e, SingleNode<EulaNotificationComponent> eulaNotification, [Combine] SingleNode<InviteDialogComponent> inviteDialog) {
            inviteDialog.component.OnNo();
        }

        [OnEventFire]
        public void HideInviteDialogOnPP(NodeAddedEvent e, SingleNode<PrivacyPolicyNotificationComponent> ppNotification, [Combine] SingleNode<InviteDialogComponent> inviteDialog) {
            inviteDialog.component.OnNo();
        }

        [OnEventFire]
        public void OnDialogConfirm(DialogConfirmEvent e, SingleNode<InviteToLobbyDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user) {
            AcceptInviteEvent acceptInviteEvent = new();
            acceptInviteEvent.lobbyId = dialog.component.lobbyId;
            acceptInviteEvent.engineId = dialog.component.engineId;
            AcceptInviteEvent acceptInviteEvent2 = acceptInviteEvent;
            ExitOtherLobbyAndAcceptInviteEvent exitOtherLobbyAndAcceptInviteEvent = new();
            exitOtherLobbyAndAcceptInviteEvent.AcceptInviteEvent = acceptInviteEvent2;
            ExitOtherLobbyAndAcceptInviteEvent eventInstance = exitOtherLobbyAndAcceptInviteEvent;
            NewEvent(eventInstance).Attach(user).Attach(dialog).Schedule();
        }

        [OnEventFire]
        public void ExitLobbyOrAcceptInvite(ExitOtherLobbyAndAcceptInviteEvent e, SingleNode<SelfUserComponent> user, [Combine] SingleNode<InviteToLobbyDialogComponent> dialog,
            [JoinAll] Optional<LobbyNode> lobby) {
            if (lobby.IsPresent() && lobby.Get().battleLobbyGroup.Key != e.AcceptInviteEvent.lobbyId) {
                dialog.Entity.AddComponent(new WaitingLobbyExitComponent {
                    AcceptInviteEvent = e.AcceptInviteEvent
                });

                ScheduleEvent<ClientExitLobbyEvent>(lobby.Get());
            } else {
                ScheduleEvent(e.AcceptInviteEvent, user);
            }
        }

        [OnEventFire]
        public void AcceptInviteAfterExitLobby(NodeRemoveEvent e, LobbyNode lobby, [JoinAll] [Combine] SingleNode<WaitingLobbyExitComponent> dialog,
            [JoinAll] SingleNode<SelfUserComponent> user) {
            AcceptInviteEvent acceptInviteEvent = dialog.Entity.GetComponent<WaitingLobbyExitComponent>().AcceptInviteEvent;
            dialog.Entity.RemoveComponent<WaitingLobbyExitComponent>();
            ScheduleEvent(acceptInviteEvent, user);
        }

        public class WaitingInviteAnswerUserLabelNode : Node {
            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public WaitingForInviteToLobbyAnswerUIComponent waitingForInviteToLobbyAnswerUi;
        }

        [Not(typeof(SelfUserComponent))]
        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfLobbyUserNode : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;

            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class LobbyUserNode : UserNode {
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class DialogsNode : Node {
            public Dialogs60Component dialogs60;
        }

        public class LobbyNode : Node {
            public BattleLobbyComponent battleLobby;

            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class InviteToLobbyButtonNode : Node {
            public SendInviteToLobbyButtonComponent sendInviteToLobbyButton;

            public UserGroupComponent userGroup;
        }
    }
}