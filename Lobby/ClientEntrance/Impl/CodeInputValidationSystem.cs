using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientEntrance.Impl {
    public class CodeInputValidationSystem : ECSSystem {
        [OnEventFire]
        public void ShowRegistrationCodeDoesNotExistHint(RegistrationCodeDoesNotExistEvent e,
            SingleNode<ClientSessionComponent> clientSession, [JoinAll] CodeInputFieldNode inviteNode,
            [JoinAll] EnterRegistrationCodeScreenNode enterRegistrationCodeScreen) {
            inviteNode.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            inviteNode.inputField.ErrorMessage = enterRegistrationCodeScreen.enterRegistrationCodeScreenText.Error;
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(InputFieldValueChangedEvent e, CodeInputFieldAcceptableNode inputField) {
            if (inputField.inputField.Input.Length == 0) {
                inputField.interactivityPrerequisiteESM.Esm
                    .ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(NodeAddedEvent e, InvalidStateNode inputField) => inputField
            .interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();

        [OnEventFire]
        public void SwitchToAcceptableState(InputFieldValueChangedEvent e, CodeInputFieldNotAcceptableNode inputField) {
            if (inputField.inputField.Input.Length > 0) {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchInputToNormalState(InputFieldValueChangedEvent e, CodeInputFieldInvalidStateNode inputField) =>
            inputField.esm.Esm.ChangeState<InputFieldStates.NormalState>();

        public class CodeInputFieldAcceptableNode : Node {
            public AcceptableStateComponent acceptableState;
            public CodeInputFieldComponent codeInputField;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
        }

        public class CodeInputFieldNotAcceptableNode : Node {
            public CodeInputFieldComponent codeInputField;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;

            public NotAcceptableStateComponent notAcceptableState;
        }

        public class CodeInputFieldInvalidStateNode : Node {
            public CodeInputFieldComponent codeInputField;

            public ESMComponent esm;

            public InputFieldInvalidStateComponent inputFieldInvalidState;
        }

        public class CodeInputFieldNode : Node {
            public CodeInputFieldComponent codeInputField;

            public ESMComponent esm;

            public InputFieldComponent inputField;
        }

        public class InvalidStateNode : Node {
            public CodeInputFieldComponent codeInputField;

            public InputFieldInvalidStateComponent inputFieldInvalidState;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
        }

        public class EnterRegistrationCodeScreenNode : Node {
            public EnterRegistrationCodeScreenComponent enterRegistrationCodeScreen;

            public EnterRegistrationCodeScreenTextComponent enterRegistrationCodeScreenText;
        }
    }
}