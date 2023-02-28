using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

public class QuickGameStepValidator : ECSBehaviour, ITutorialShowStepValidator {
    [Inject] public new static EngineServiceInternal EngineService { get; set; }

    public bool ShowAllowed(long stepId) {
        GetEnergyCountTutorialValidationDataEvent getEnergyCountTutorialValidationDataEvent = new();
        ScheduleEvent(getEnergyCountTutorialValidationDataEvent, EngineService.EntityStub);
        return getEnergyCountTutorialValidationDataEvent.Quantums <= 0;
    }
}