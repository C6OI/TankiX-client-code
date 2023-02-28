using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialOpenContainerHandler : TutorialStepHandler {
        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            OpenTutorialContainerEvent openTutorialContainerEvent = new();
            openTutorialContainerEvent.StepId = data.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId;
            openTutorialContainerEvent.ItemId = data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId;
            OpenTutorialContainerEvent eventInstance = openTutorialContainerEvent;
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            StepComplete();
        }
    }
}