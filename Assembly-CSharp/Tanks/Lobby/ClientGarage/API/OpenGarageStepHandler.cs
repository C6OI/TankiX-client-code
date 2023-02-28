using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class OpenGarageStepHandler : TutorialStepHandler {
        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            MainScreenComponent.Instance.ShowParts();
            StepComplete();
        }
    }
}