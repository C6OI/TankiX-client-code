using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ForceEnterEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void MarkSession(NodeRemoveEvent e, SingleNode<RequireEnterEmailComponent> user) =>
            ScheduleEvent<ShowScreenLeftEvent<HomeScreenComponent>>(user);

        [OnEventComplete]
        public void CancelGoHome(NodeAddedEvent e, SingleNode<HomeScreenComponent> home,
            SingleNode<RequireEnterEmailComponent> require) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<ForceEnterEmailScreenComponent>>(home);

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, Node any, [JoinAll] LockedForceEnterScreenNode screen,
            [JoinByScreen] EmailInputNode emailInput) {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedForceEnterScreenNode screen,
            [JoinByScreen] EmailInputNode emailInput) {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        public class LockedForceEnterScreenNode : Node {
            public ForceEnterEmailScreenComponent forceEnterEmailScreen;

            public LockedScreenComponent lockedScreen;
        }

        public class EmailInputNode : Node {
            public EmailInputFieldComponent emailInputField;

            public ESMComponent esm;

            public InputFieldComponent inputField;
        }
    }
}