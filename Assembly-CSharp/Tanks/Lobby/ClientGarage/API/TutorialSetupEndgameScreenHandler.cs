using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialSetupEndgameScreenHandler : TutorialStepHandler {
        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            gameObject.SetActive(true);
            StepComplete();
        }
    }
}