using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientEntrance.Impl {
    public class EnterRegistrationCodeScreenSystem : ECSSystem {
        [OnEventFire]
        public void SwitchToEnterRegistrationCode(NodeAddedEvent e,
            SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen) {
            ShowNameHints(enterRegistrationCodeScreen.component, false);
            ScheduleEvent<CheckReservationCodeRequestEvent>(enterRegistrationCodeScreen);
        }

        [OnEventFire]
        public void SwitchToEnterRegistrationCode(NodeAddedEvent e,
            SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen, SelfUserNode selfUser) {
            ShowNameHints(enterRegistrationCodeScreen.component, false);
            ScheduleEvent<CheckReservationCodeRequestEvent>(enterRegistrationCodeScreen);
        }

        [OnEventFire]
        public void SwitchToEnterRegistrationCode(CheckReservationCodeRequestEvent e,
            SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen,
            [JoinAll] ClientSessionWithReservationCodeRequestNode clientSessionNode) =>
            ShowNameHints(enterRegistrationCodeScreen.component, true);

        void ShowNameHints(EnterRegistrationCodeScreenComponent enterRegistrationCodeScreen, bool visible) {
            enterRegistrationCodeScreen.ChangeNameLinkActivity = visible;
            enterRegistrationCodeScreen.ReservedNameHintActivity = visible;
            enterRegistrationCodeScreen.BackButtonActivity = !visible;
            enterRegistrationCodeScreen.ReservationCodeHintActivity = !visible;
        }

        [OnEventFire]
        public void SwitchToChangeUserName(ButtonClickEvent e, SingleNode<ChangeUserNameLinkComponent> node,
            [JoinAll] SingleNode<ClientSessionComponent> session) =>
            ScheduleEvent<ShowScreenDownEvent<ChangeUserNameScreenComponent>>(node);

        [OnEventFire]
        public void ClientPassRegistration(NodeAddedEvent e, SelfUserNode selfUser,
            [JoinByUser] ClientSessionWithoutReservationCodeRequestNode clientSessionNode) =>
            ScheduleEvent<ShowLobbyScreenEvent>(selfUser);

        [OnEventFire]
        public void SubmitReservationCodeToServer(ButtonClickEvent e,
            SingleNode<RegistrationCodeSubmitButtonComponent> submitButton, [JoinAll] ClientSessionNode clientSession,
            [JoinAll] SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen) {
            ScheduleEvent(new CheckEntranceCodeEvent(enterRegistrationCodeScreen.component.InputtedRegistrationCode.Trim()),
                clientSession);

            enterRegistrationCodeScreen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void TurnOffWaitingCover(RegistrationCodeDoesNotExistEvent e,
            SingleNode<ClientSessionComponent> clientSession,
            [JoinAll] SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen) =>
            enterRegistrationCodeScreen.Entity.RemoveComponent<LockedScreenComponent>();

        [OnEventFire]
        public void EntereReservationCodeSuccessful(NodeAddedEvent e, ClientSessionWithUidReservationCodeNode clientSession,
            [JoinAll] SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen) {
            enterRegistrationCodeScreen.Entity.RemoveComponent<LockedScreenComponent>();
            ScheduleEvent<ShowScreenDownEvent<RegistrationScreenComponent>>(clientSession);
        }

        [OnEventFire]
        public void EntereCBQRegistrationCodeSuccessful(NodeAddedEvent e, ClientSessionWithCbqCodeNode clientSession,
            [JoinAll] SingleNode<EnterRegistrationCodeScreenComponent> enterRegistrationCodeScreen) {
            enterRegistrationCodeScreen.Entity.RemoveComponent<LockedScreenComponent>();
            ScheduleEvent<ShowScreenDownEvent<RegistrationScreenComponent>>(clientSession);
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class ClientSessionNode : Node {
            public ClientSessionComponent clientSession;
        }

        [Not(typeof(ReservationCodeRequiredComponent))]
        public class ClientSessionWithoutReservationCodeRequestNode : ClientSessionNode { }

        public class ClientSessionWithReservationCodeRequestNode : ClientSessionNode {
            public ReservationCodeRequiredComponent reservationCodeRequired;
        }

        public class ClientSessionWithUidReservationCodeNode : ClientSessionNode {
            public UidReservationCodeComponent uidReservationCode;
        }

        public class ClientSessionWithCbqCodeNode : ClientSessionNode {
            public CbqCodeComponent cbqCode;
        }

        public class CheckReservationCodeRequestEvent : Event { }
    }
}