using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class AddItemStepHandler : TutorialStepHandler {
        [SerializeField] bool completeOnResponse;

        public long stepId => tutorialData.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId;

        public override void RunStep(TutorialData data) {
            TutorialCanvas.Instance.BlockInteractable();
            base.RunStep(data);
            gameObject.SetActive(true);
        }

        public void Success(long stepId) {
            if (completeOnResponse) {
                TutorialCanvas.Instance.UnblockInteractable();
                StepComplete();
            } else {
                tutorialData.ContinueOnClick = true;
                TutorialCanvas.Instance.SetupActivePopup(tutorialData);
            }

            gameObject.SetActive(false);
        }

        public void Fail(long stepId) {
            if (completeOnResponse) {
                TutorialCanvas.Instance.UnblockInteractable();
                StepComplete();
            } else {
                tutorialData.ContinueOnClick = true;
                TutorialCanvas.Instance.SetupActivePopup(tutorialData);
            }

            gameObject.SetActive(false);
        }
    }
}