using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class InviteScreenSystem : ECSSystem {
        const string SECRET = "StormtrooperIsADick";

        [OnEventFire]
        public void SubmitInviteToServer(ButtonClickEvent e, SingleNode<InviteSubmitButtonComponent> loginButton, [JoinAll] ClientSessionInviteNode clientSession,
            [JoinAll] SingleNode<InviteScreenComponent> inviteScreen) {
            inviteScreen.component.InviteField.Input = inviteScreen.component.InviteField.Input.Trim();
            clientSession.invite.InviteCode = inviteScreen.component.InviteField.Input;
            ScheduleEvent<InviteEnteredEvent>(clientSession);
            inviteScreen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void SwitchToEntrance(ButtonClickEvent e, SingleNode<SwitchToEntranceButtonComponent> switchToEntrance, [JoinAll] ClientSessionInviteNode clientSession) {
            ScheduleEvent<ReleaseInviteReservationEvent>(clientSession);
        }

        [OnEventFire]
        public void ShowInviteDoesNotExistHint(InviteDoesNotExistEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<InviteScreenComponent> inviteScreen) {
            inviteScreen.Entity.RemoveComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void SwitchToInvite(ButtonClickEvent e, SingleNode<SwitchToRegistrationButtonComponent> node, [JoinAll] SingleNode<InviteComponent> clientSession) {
            ScheduleEvent<ShowScreenDownEvent<InviteScreenComponent>>(node);
        }

        [OnEventComplete]
        public void SwitchToInvite(NodeAddedEvent e, SingleNode<EntranceScreenComponent> entranceScreen, [JoinAll] SingleNode<InviteComponent> clientSession) {
            if (clientSession.component.ShowScreenOnEntrance && !clientSession.Entity.HasComponent<CorrectInviteComponent>() &&
                !string.Equals(clientSession.component.InviteCode, "StormtrooperIsADick", StringComparison.CurrentCultureIgnoreCase)) {
                entranceScreen.component.Login = string.Empty;
                NewEvent<ShowScreenDownEvent<InviteScreenComponent>>().Attach(clientSession).ScheduleDelayed(1f);
            }
        }

        [OnEventFire]
        public void ShowRegistrationScreen(CommenceRegistrationEvent e, SingleNode<ClientSessionComponent> clientSession) {
            ScheduleEvent<ShowScreenDownEvent<RegistrationScreenComponent>>(clientSession);
            clientSession.Entity.AddComponentIfAbsent<CorrectInviteComponent>();
        }

        public class ClientSessionInviteNode : Node {
            public ClientSessionComponent clientSession;
            public InviteComponent invite;
        }
    }
}