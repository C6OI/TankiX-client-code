using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class InputFieldStates {
        public const string NORMAL_ANIMATOR_STATE_TRIGGER = "Reset";

        public const string INVALID_ANIMATOR_STATE_TRIGGER = "Invalid";

        public const string VALID_ANIMATOR_STATE_TRIGGER = "Valid";

        public const string AWAIT_ANIMATOR_STATE_TRIGGER = "Await";

        public const string HAS_MESSAGE_ANIMATOR_PROPERTY = "HasMessage";

        public class NormalState : Node {
            public InputFieldNormalStateComponent inputFieldNormalState;
        }

        public class AwaitState : Node {
            public InputFieldAwaitStateComponent inputFieldAwaitState;
        }

        public class ValidState : Node {
            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class InvalidState : Node {
            public InputFieldInvalidStateComponent inputFieldInvalidState;
        }
    }
}