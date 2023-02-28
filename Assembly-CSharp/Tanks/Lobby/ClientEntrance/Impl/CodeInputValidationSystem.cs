using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class CodeInputValidationSystem : ECSSystem {
        [OnEventFire]
        public void SwitchToNotAcceptableState(InputFieldValueChangedEvent e, CodeInputFieldAcceptableNode inputField) {
            if (inputField.inputField.Input.Length == 0) {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(NodeAddedEvent e, InvalidStateNode inputField) {
            inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
        }

        [OnEventFire]
        public void SwitchToAcceptableState(InputFieldValueChangedEvent e, CodeInputFieldNotAcceptableNode inputField) {
            if (inputField.inputField.Input.Length > 0) {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchInputToNormalState(InputFieldValueChangedEvent e, CodeInputFieldInvalidStateNode inputField) {
            inputField.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

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
    }
}