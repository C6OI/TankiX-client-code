using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class HighlightModuleAndCryStepHandler : TutorialStepHandler {
        [SerializeField] RectTransform popupPosition;

        [SerializeField] RectTransform crysRect;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            TutorialCanvas.Instance.AddAdditionalMaskRect(crysRect.gameObject);
            data.PopupPositionRect = popupPosition;
            data.ContinueOnClick = false;
            TutorialCanvas.Instance.Show(data, true);
            Invoke("Complete", 1f);
        }

        void Complete() {
            StepComplete();
        }
    }
}