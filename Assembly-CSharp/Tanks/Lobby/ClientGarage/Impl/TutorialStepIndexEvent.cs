using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialStepIndexEvent : Event {
        public int CurrentStepNumber { get; set; }

        public int StepCountInTutorial { get; set; }
    }
}