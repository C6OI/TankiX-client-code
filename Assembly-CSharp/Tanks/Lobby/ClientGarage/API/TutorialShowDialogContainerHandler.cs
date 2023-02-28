using System;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialShowDialogContainerHandler : TutorialStepHandler {
        public int chestCount;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            OpenTutorialContainerDialogEvent openTutorialContainerDialogEvent = new();
            openTutorialContainerDialogEvent.StepId = tutorialData.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId;
            openTutorialContainerDialogEvent.ItemId = tutorialData.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId;
            openTutorialContainerDialogEvent.ItemsCount = chestCount;
            OpenTutorialContainerDialogEvent openTutorialContainerDialogEvent2 = openTutorialContainerDialogEvent;
            ScheduleEvent(openTutorialContainerDialogEvent2, EngineService.EntityStub);
            openTutorialContainerDialogEvent2.dialog.Message = tutorialData.Message;
            TutorialContainerDialog dialog = openTutorialContainerDialogEvent2.dialog;
            dialog.dialogClosed = (Action)Delegate.Combine(dialog.dialogClosed, new Action(Complete));
        }

        void Complete() {
            StepComplete();
        }
    }
}