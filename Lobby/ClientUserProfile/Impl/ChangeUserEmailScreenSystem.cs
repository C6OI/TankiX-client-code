using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ChangeUserEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void RequestChangeEmail(ButtonClickEvent e, SingleNode<ChangeUserEmailButtonComponent> button,
            [JoinByScreen] SingleNode<ScreenComponent> screen, [JoinByScreen] EmailInputNode emailInput,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            ScheduleEvent(new RequestChangeUserEmailEvent(emailInput.inputField.Input), selfUser);
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen,
            [JoinByScreen] EmailInputNode emailInput) {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen,
            [JoinByScreen] EmailInputNode emailInput) {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen) =>
            ScheduleEvent<ShowScreenLeftEvent<ConfirmUserEmailScreenComponent>>(screen);

        public class EmailInputNode : Node {
            public EmailInputFieldComponent emailInputField;

            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class LockedChangeUserEmailScreenNode : Node {
            public ChangeUserEmailScreenComponent changeUserEmailScreen;

            public LockedScreenComponent lockedScreen;
        }
    }
}