using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.API {
    public class GameModeTutorialStepHandler : TutorialStepHandler {
        [SerializeField] TutorialShowTriggerComponent nextStepTrigger;

        [SerializeField] Transform buttonContainer;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            Button componentInChildren = buttonContainer.GetComponentInChildren<Button>();

            if (componentInChildren != null) {
                nextStepTrigger.SetSeleectable(componentInChildren);
            }

            StepComplete();
        }
    }
}