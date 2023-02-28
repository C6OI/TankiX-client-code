using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class InviteInputValidationSystem : ECSSystem {
        [OnEventFire]
        public void ShowInviteDoesNotExistHint(InviteDoesNotExistEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] InviteInputFieldNode inviteNode,
            [JoinAll] InviteScreenNode inviteScreenNode) {
            inviteNode.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            inviteNode.inputField.ErrorMessage = inviteScreenNode.inviteScreenText.Error;
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(InputFieldValueChangedEvent e, InviteInputFieldAcceptableNode inputField) {
            if (inputField.inputField.Input.Length == 0) {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(NodeAddedEvent e, InvalidStateNode inputField) {
            inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
        }

        [OnEventFire]
        public void SwitchToAcceptableState(InputFieldValueChangedEvent e, InviteInputFieldNotAcceptableNode inputField) {
            if (inputField.inputField.Input.Length > 0) {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchInputToNormalState(InputFieldValueChangedEvent e, InputFieldInvalidStateNode inputField) {
            inputField.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        public class InviteInputFieldAcceptableNode : Node {
            public AcceptableStateComponent acceptableState;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
            public InviteInputFieldComponent inviteInputField;
        }

        public class InviteInputFieldNotAcceptableNode : Node {
            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
            public InviteInputFieldComponent inviteInputField;

            public NotAcceptableStateComponent notAcceptableState;
        }

        public class InputFieldInvalidStateNode : Node {
            public ESMComponent esm;

            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public InviteInputFieldComponent inviteInputField;
        }

        public class InviteInputFieldNode : Node {
            public ESMComponent esm;

            public InputFieldComponent inputField;
            public InviteInputFieldComponent inviteInputField;
        }

        public class InviteScreenNode : Node {
            public InviteScreenComponent inviteScreen;

            public InviteScreenTextComponent inviteScreenText;
        }

        public class InvalidStateNode : Node {
            public InputFieldInvalidStateComponent inputFieldInvalidState;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
            public InviteInputFieldComponent inviteInputField;
        }
    }
}