using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class ChangeUserEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen, [JoinByScreen] EmailInputNode emailInput) {
            if (screen.Entity.HasComponent<LockedScreenComponent>()) {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }

            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen, [JoinByScreen] EmailInputNode emailInput) {
            if (screen.Entity.HasComponent<LockedScreenComponent>()) {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }

            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen) {
            ScheduleEvent<ShowScreenLeftEvent<ConfirmUserEmailScreenComponent>>(screen);
        }

        [OnEventComplete]
        public void HideHint(NodeAddedEvent e, SingleNode<ChangeUserEmailScreenComponent> screen, SelfUserWithConfirmedEmailNode user) {
            screen.component.DeactivateHint();
        }

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

        public class SelfUserWithConfirmedEmailNode : Node {
            public ConfirmedUserEmailComponent confirmedUserEmail;

            public SelfUserComponent selfUser;
        }
    }
}