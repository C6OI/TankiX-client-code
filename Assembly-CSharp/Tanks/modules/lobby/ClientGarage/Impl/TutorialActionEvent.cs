using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace tanks.modules.lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1506070003266L)]
    public class TutorialActionEvent : Event {
        public TutorialActionEvent() { }

        public TutorialActionEvent(long tutorialId, long stepId, TutorialAction action) {
            TutorialId = tutorialId;
            StepId = stepId;
            Action = action;
        }

        public long TutorialId { get; set; }

        public long StepId { get; set; }

        public TutorialAction Action { get; set; }
    }
}