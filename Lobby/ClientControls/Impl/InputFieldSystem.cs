using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientControls.Impl {
    public class InputFieldSystem : ECSSystem {
        [OnEventFire]
        public void InitESM(NodeAddedEvent e, SingleNode<InputFieldComponent> node) {
            ESMComponent eSMComponent = new();
            node.Entity.AddComponent(eSMComponent);
            EntityStateMachine esm = eSMComponent.Esm;
            esm.AddState<InputFieldStates.NormalState>();
            esm.AddState<InputFieldStates.InvalidState>();
            esm.AddState<InputFieldStates.ValidState>();
            esm.AddState<InputFieldStates.AwaitState>();
            esm.ChangeState<InputFieldStates.NormalState>();
        }

        [OnEventFire]
        public void ClearInputOnShow(NodeAddedEvent e, ClearInputFieldNode inputField) =>
            inputField.inputField.Input = string.Empty;

        [OnEventFire]
        public void HandleInvalidState(NodeAddedEvent e, InputFieldInvalidNode node) =>
            node.inputField.Animator.SetTrigger("Invalid");

        [OnEventFire]
        public void HandleValidState(NodeAddedEvent e, InputFieldValidNode node) =>
            node.inputField.Animator.SetTrigger("Valid");

        [OnEventFire]
        public void HandleAwaitState(NodeAddedEvent e, InputFieldAwaitNode node) =>
            node.inputField.Animator.SetTrigger("Await");

        [OnEventFire]
        public void HandleNormalState(NodeAddedEvent e, InputFieldNormalNode node) =>
            node.inputField.Animator.SetTrigger("Reset");

        public class InputFieldInvalidNode : Node {
            public InputFieldComponent inputField;
            public InputFieldInvalidStateComponent inputFieldInvalidState;
        }

        public class InputFieldValidNode : Node {
            public InputFieldComponent inputField;
            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class InputFieldNormalNode : Node {
            public InputFieldComponent inputField;
            public InputFieldNormalStateComponent inputFieldNormalState;
        }

        public class InputFieldAwaitNode : Node {
            public InputFieldComponent inputField;
            public InputFieldAwaitStateComponent inputFieldAwaitState;
        }

        public class ClearInputFieldNode : Node {
            public ClearInputOnShowComponent clearInputOnShow;

            public InputFieldComponent inputField;
        }
    }
}