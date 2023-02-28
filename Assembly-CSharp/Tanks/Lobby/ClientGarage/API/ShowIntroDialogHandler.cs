using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class ShowIntroDialogHandler : TutorialStepHandler {
        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            CheckForSkipTutorial checkForSkipTutorial = new();
            EngineService.Engine.ScheduleEvent(checkForSkipTutorial, data.TutorialStep);
            TutorialCanvas.Instance.ShowIntroDialog(data, checkForSkipTutorial.canSkipTutorial);
        }
    }
}