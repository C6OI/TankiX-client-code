using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class ShowTutorialRewardsStepHandler : TutorialStepHandler {
        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            EngineService.Engine.ScheduleEvent<ShowTutorialRewardsEvent>(data.TutorialStep);
            StepComplete();
        }
    }
}