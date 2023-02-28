using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class SelectCryStepHandler : TutorialStepHandler {
        [SerializeField] RectTransform popupPositionRect;

        [SerializeField] RectTransform highlightedRects;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            CheckBoughtItemEvent checkBoughtItemEvent = new(data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId);
            EngineService.Engine.ScheduleEvent(checkBoughtItemEvent, EngineService.EntityStub);

            if (checkBoughtItemEvent.TutorialItemAlreadyBought) {
                EngineService.Engine.ScheduleEvent<CompleteTutorialByEscEvent>(data.TutorialStep);
                Complete();
                return;
            }

            data.PopupPositionRect = popupPositionRect;
            data.ContinueOnClick = false;
            TutorialCanvas.Instance.Show(data, true, new GameObject[1] { highlightedRects.gameObject });
            gameObject.SetActive(true);
            Invoke("Complete", 1f);
        }

        void Complete() {
            StepComplete();
        }
    }
}