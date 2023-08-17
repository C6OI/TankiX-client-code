using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.API {
    public class ServiceMessageStates {
        public class ServiceMessageVisibleState : Node {
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
        }

        public class ServiceMessageHiddenState : Node {
            public ServiceMessageHiddenStateComponent serviceMessageHiddenState;
        }
    }
}