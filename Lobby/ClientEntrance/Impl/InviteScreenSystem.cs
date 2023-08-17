using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientEntrance.Impl {
    public class InviteScreenSystem : ECSSystem {
        [OnEventFire]
        public void SubmitInviteToServer(ButtonClickEvent e, SingleNode<InviteSubmitButtonComponent> loginButton,
            [JoinAll] ClientSessionInviteNode clientSession, [JoinAll] SingleNode<InviteScreenComponent> inviteScreen) {
            inviteScreen.component.InviteField.Input = inviteScreen.component.InviteField.Input.Trim();
            clientSession.invite.InviteCode = inviteScreen.component.InviteField.Input;
            ScheduleEvent<InviteEnteredEvent>(clientSession);
            inviteScreen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void SwitchToEntrance(ButtonClickEvent e, SingleNode<SwitchToEntranceButtonComponent> switchToEntrance,
            [JoinAll] ClientSessionInviteNode clientSession) => ScheduleEvent<ReleaseInviteReservationEvent>(clientSession);

        [OnEventFire]
        public void ShowInviteDoesNotExistHint(InviteDoesNotExistEvent e, SingleNode<ClientSessionComponent> clientSession,
            [JoinAll] SingleNode<InviteScreenComponent> inviteScreen) =>
            inviteScreen.Entity.RemoveComponent<LockedScreenComponent>();

        [OnEventFire]
        public void ShowRegistrationScreen(CommenceRegistrationEvent e, SingleNode<ClientSessionComponent> clientSession) =>
            ScheduleEvent<ShowScreenDownEvent<RegistrationWithReservationCodeScreenComponent>>(clientSession);

        [OnEventFire]
        public void EntereCBQRegistrationCodeSuccessful(NodeAddedEvent e, ClientSessionWithCbqCodeNode clientSession,
            [JoinAll] SingleNode<InviteScreenComponent> inviteScreen) {
            inviteScreen.Entity.RemoveComponent<LockedScreenComponent>();
            ScheduleEvent<ShowScreenDownEvent<RegistrationScreenComponent>>(clientSession);
        }

        public class InviteScreenNode : Node {
            public InviteScreenComponent inviteScreen;

            public InviteScreenTextComponent inviteScreenText;
        }

        public class ClientSessionInviteNode : Node {
            public ClientSessionComponent clientSession;
            public InviteComponent invite;
        }

        public class ClientSessionWithCbqCodeNode : Node {
            public CbqCodeComponent cbqCode;
        }
    }
}