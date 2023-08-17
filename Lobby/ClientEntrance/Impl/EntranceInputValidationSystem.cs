using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientEntrance.Impl {
    public class EntranceInputValidationSystem : InputValidationSystem {
        [OnEventFire]
        public void ValidateLogin(InputFieldValueChangedEvent e, LoginInputFieldNode loginInput,
            [JoinAll] SingleNode<EntranceValidationRulesComponent> validationRules) => ValidateInputAfterChanging(
            loginInput.inputField,
            loginInput.esm,
            loginInput.interactivityPrerequisiteESM,
            validationRules.component.maxEmailLength);

        [Mandatory]
        [OnEventFire]
        public void HandleInvalidUid(UidInvalidEvent e, SingleNode<ClientSessionComponent> session,
            [JoinAll] LoginInputFieldNode loginInput,
            [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText) => SetInvalidAndNotAccetableState(
            loginInput.inputField,
            loginInput.esm,
            entranceScreenText.component.IncorrectLogin,
            loginInput.interactivityPrerequisiteESM);

        [OnEventFire]
        public void HandleInvalidEmail(EmailInvalidEvent e, SingleNode<ClientSessionComponent> session,
            [JoinAll] LoginInputFieldNode loginInput,
            [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText) => SetInvalidAndNotAccetableState(
            loginInput.inputField,
            loginInput.esm,
            entranceScreenText.component.IncorrectLogin,
            loginInput.interactivityPrerequisiteESM);

        [OnEventFire]
        [Mandatory]
        public void HandleUnconfirmedEmail(EmailNotConfirmedEvent e, SingleNode<ClientSessionComponent> session,
            [JoinAll] LoginInputFieldNode loginInput,
            [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText) => SetInvalidAndNotAccetableState(
            loginInput.inputField,
            loginInput.esm,
            entranceScreenText.component.UnconfirmedEmail,
            loginInput.interactivityPrerequisiteESM);

        void SetInvalidAndNotAccetableState(InputFieldComponent inputField, ESMComponent inputESM, string errorMessage,
            InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM) {
            inputESM.Esm.ChangeState<InputFieldStates.InvalidState>();
            inputField.ErrorMessage = errorMessage;
            SetNotAcceptableState(interactivityPrerequisiteESM);
        }

        [OnEventFire]
        public void ValidatePassword(InputFieldValueChangedEvent e, PasswordInputFieldNode passwordInput,
            [JoinAll] SingleNode<EntranceValidationRulesComponent> validationRules) => ValidateInputAfterChanging(
            passwordInput.inputField,
            passwordInput.esm,
            passwordInput.interactivityPrerequisiteESM,
            validationRules.component.maxPasswordLength);

        [OnEventFire]
        [Mandatory]
        public void HandleInvalidPassword(InvalidPasswordEvent e, SingleNode<ClientSessionComponent> session,
            [JoinAll] PasswordInputFieldNode passwordInput,
            [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText) => SetInvalidAndNotAccetableState(
            passwordInput.inputField,
            passwordInput.esm,
            entranceScreenText.component.IncorrectPassword,
            passwordInput.interactivityPrerequisiteESM);

        [OnEventFire]
        public void ValidateCaptchaInput(InputFieldValueChangedEvent e, CaptchaInputFieldNode inputField,
            [JoinAll] SingleNode<EntranceValidationRulesComponent> validationRules) => ValidateInputAfterChanging(
            inputField.inputField,
            inputField.esm,
            inputField.interactivityPrerequisiteESM,
            validationRules.component.maxCaptchaLength);

        [Mandatory]
        [OnEventFire]
        public void HandleInvalidCaptcha(CaptchaInvalidEvent e, SingleNode<ClientSessionComponent> session,
            [JoinAll] CaptchaInputFieldNode captchaField,
            [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText) => SetInvalidAndNotAccetableState(
            captchaField.inputField,
            captchaField.esm,
            entranceScreenText.component.IncorrectCaptcha,
            captchaField.interactivityPrerequisiteESM);

        void ValidateInputAfterChanging(InputFieldComponent input, ESMComponent inputStateESM,
            InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM, int maxLength) {
            string input2 = input.Input;

            if (string.IsNullOrEmpty(input2)) {
                SetNotAcceptableState(interactivityPrerequisiteESM);
            } else {
                if (input2.Length > maxLength) {
                    input.Input = input2.Remove(maxLength);
                }

                interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }

            inputStateESM.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        void SetNotAcceptableState(InteractivityPrerequisiteESMComponent prerequisiteESM) =>
            prerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();

        public class LoginInputFieldNode : BaseInputFieldNode<LoginInputFieldComponent> { }

        public class PasswordInputFieldNode : BaseInputFieldNode<PasswordInputFieldComponent> { }

        public class CaptchaInputFieldNode : BaseInputFieldNode<CaptchaInputFieldComponent> { }
    }
}