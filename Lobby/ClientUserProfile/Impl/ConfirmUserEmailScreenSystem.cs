using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ConfirmUserEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void InsertUserEmail(NodeAddedEvent e, SingleNode<ConfirmUserEmailScreenComponent> screen,
            SelfUserWithUnconfirmedEmailNode user) {
            ConfirmUserEmailScreenComponent component = screen.component;

            component.ConfirmationHintWithUserEmail = component.ConfirmationHint.Replace("%EMAIL%",
                string.Format("<color=#{1}>{0}</color>",
                    user.unconfirmedUserEmail.Email,
                    component.EmailColor.ToHexString()));
        }

        [OnEventFire]
        public void ActivateCancelButton(NodeAddedEvent e, SingleNode<ConfirmUserEmailScreenComponent> screen,
            SelfUserWithConfirmedEmailNode user) => screen.component.ActivateCancel();

        [OnEventFire]
        public void SendConfirmationCode(ButtonClickEvent e, SingleNode<ConfirmUserEmailButtonComponent> button,
            [Mandatory] [JoinByScreen] SingleNode<ConfirmUserEmailScreenComponent> screen,
            [JoinByScreen] [Mandatory] UserEmailConfirmationCodeInputNode codeInput,
            [JoinByScreen] [Mandatory] SubscribeCheckboxNode subscribeCheckbox,
            [JoinAll] [Mandatory] SelfUserWithUnconfirmedEmailNode user) {
            RequestUserEmailConfirmationEvent requestUserEmailConfirmationEvent = new();
            requestUserEmailConfirmationEvent.Code = codeInput.inputField.Input;
            requestUserEmailConfirmationEvent.Subscribe = subscribeCheckbox.checkbox.IsChecked;
            RequestUserEmailConfirmationEvent eventInstance = requestUserEmailConfirmationEvent;
            ScheduleEvent(eventInstance, user);
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void GoToViewUserEmailScreen(UserEmailConfirmationCodeValidEvent e, Node any,
            [JoinAll] LockedConfirmUserEmailScreenNode screen) =>
            ScheduleEvent<ShowScreenLeftEvent<ViewUserEmailScreenComponent>>(screen);

        [OnEventFire]
        public void InputToInvalid(UserEmailConfirmationCodeInvalidEvent e, Node any,
            [JoinAll] LockedConfirmUserEmailScreenNode screen, [JoinByScreen] UserEmailConfirmationCodeInputNode codeInput) {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
            codeInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            codeInput.inputField.ErrorMessage = screen.confirmUserEmailScreen.InvalidCodeMessage;
        }

        [OnEventFire]
        public void GoToChangeUserEmailScreen(EmailOccupiedEvent e, Node any,
            [JoinAll] LockedConfirmUserEmailScreenNode screen,
            [JoinByScreen] UserEmailConfirmationCodeInputNode codeInput) =>
            ScheduleEvent<ShowScreenLeftEvent<ChangeUserEmailScreenComponent>>(screen);

        [OnEventFire]
        public void ToNormalState(InputFieldValueChangedEvent e, UserEmailConfirmationCodeInputNode node) {
            if (string.IsNullOrEmpty(node.inputField.Input)) {
                node.interactivityPrerequisiteEsm.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            } else {
                node.interactivityPrerequisiteEsm.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }

            node.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        [OnEventFire]
        public void SendAgain(ButtonClickEvent e, SingleNode<SendAgainButtonComponent> button,
            [JoinByScreen] SingleNode<ConfirmUserEmailScreenComponent> screen,
            [JoinAll] [Mandatory] SelfUserWithUnconfirmedEmailNode user) =>
            ScheduleEvent<RequestSendAgainConfirmationEmailEvent>(user);

        [OnEventFire]
        public void Cancel(ButtonClickEvent e, SingleNode<CancelButtonComponent> button,
            [JoinByScreen] SingleNode<ConfirmUserEmailScreenComponent> screen,
            [JoinAll] [Mandatory] SelfUserWithUnconfirmedEmailNode user) {
            ScheduleEvent<CancelChangeUserEmailEvent>(user);
            ScheduleEvent<ShowScreenRightEvent<ViewUserEmailScreenComponent>>(screen);
        }

        public class UserEmailConfirmationCodeInputNode : Node {
            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteEsm;
            public UserEmailConfirmationCodeInputFieldComponent userEmailConfirmationCodeInputField;
        }

        public class SubscribeCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public SubscribeCheckboxComponent subscribeCheckbox;
        }

        public class SelfUserWithUnconfirmedEmailNode : Node {
            public SelfUserComponent selfUser;
            public UnconfirmedUserEmailComponent unconfirmedUserEmail;
        }

        public class SelfUserWithConfirmedEmailNode : Node {
            public ConfirmedUserEmailComponent confirmedUserEmail;

            public SelfUserComponent selfUser;
        }

        public class LockedConfirmUserEmailScreenNode : Node {
            public ConfirmUserEmailScreenComponent confirmUserEmailScreen;

            public LockedScreenComponent lockedScreen;
        }
    }
}