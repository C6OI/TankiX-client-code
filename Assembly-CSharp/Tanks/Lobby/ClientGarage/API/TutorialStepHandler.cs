using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialStepHandler : BehaviourComponent {
        public TutorialData tutorialData;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public virtual void RunStep(TutorialData data) {
            tutorialData = data;
        }

        protected virtual void StepComplete() {
            ScheduleEvent<TutorialStepCompleteEvent>(tutorialData.TutorialStep);
        }
    }
}