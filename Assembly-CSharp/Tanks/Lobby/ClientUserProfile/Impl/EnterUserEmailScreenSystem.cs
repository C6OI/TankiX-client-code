using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class EnterUserEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void SwitchToRestorePassword(ButtonClickEvent e, SingleNode<RestorePasswordLinkComponent> node, [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (session.Entity.HasComponent<RestorePasswordCodeSentComponent>()) {
                ScheduleEvent<ShowScreenLeftEvent<EnterConfirmationCodeScreenComponent>>(node);
            } else {
                ScheduleEvent<ShowScreenLeftEvent<EnterUserEmailScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void RequestRestore(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<EnterUserEmailScreenComponent> screen,
            [JoinByScreen] SingleNode<InputFieldComponent> emailInput, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent(new RestorePasswordByEmailEvent {
                Email = emailInput.component.Input
            }, session);

            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<EnterUserEmailScreenComponent> screen,
            [JoinByScreen] SingleNode<ContinueButtonComponent> button) {
            screen.Entity.RemoveComponentIfPresent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void GoToEnterCodeScreen(NodeAddedEvent e, ScreenNode screen, SingleNode<RestorePasswordCodeSentComponent> email) {
            ScheduleEvent<ShowScreenLeftEvent<EnterConfirmationCodeScreenComponent>>(screen);
        }

        public class ScreenNode : Node {
            public EnterUserEmailScreenComponent enterUserEmailScreen;

            public LockedScreenComponent lockedScreen;
        }
    }
}