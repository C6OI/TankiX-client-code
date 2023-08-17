using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.API {
    public class InteractivityPrerequisiteStates {
        public class AcceptableState : Node {
            public AcceptableStateComponent acceptableState;

            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
        }

        public class NotAcceptableState : Node {
            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
            public NotAcceptableStateComponent notAcceptableState;
        }
    }
}