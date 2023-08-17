using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientUserProfile.Impl {
    public class EnterUserEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void SwitchToRestorePassword(ButtonClickEvent e, SingleNode<RestorePasswordLinkComponent> node,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (session.Entity.HasComponent<RestorePasswordUserDataComponent>()) {
                ScheduleEvent<ShowScreenLeftEvent<EnterConfirmationCodeScreenComponent>>(node);
            } else {
                ScheduleEvent<ShowScreenLeftEvent<EnterUserEmailScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void RequestRestore(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button,
            [JoinByScreen] SingleNode<EnterUserEmailScreenComponent> screen,
            [JoinByScreen] SingleNode<InputFieldComponent> emailInput,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent(new RestorePasswordByEmailEvent {
                    Email = emailInput.component.Input
                },
                session);

            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, SingleNode<ClientSessionComponent> clientSession,
            [JoinAll] SingleNode<EnterUserEmailScreenComponent> screen,
            [JoinByScreen] SingleNode<ContinueButtonComponent> button) =>
            screen.Entity.RemoveComponent<LockedScreenComponent>();

        [OnEventFire]
        public void GoToEnterCodeScreen(NodeAddedEvent e, ScreenNode screen,
            SingleNode<RestorePasswordUserDataComponent> email) =>
            ScheduleEvent<ShowScreenLeftEvent<EnterConfirmationCodeScreenComponent>>(screen);

        public class ScreenNode : Node {
            public EnterUserEmailScreenComponent enterUserEmailScreen;

            public LockedScreenComponent lockedScreen;
        }
    }
}