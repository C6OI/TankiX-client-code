using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class QuickGameCompleteTutorialValidator : ECSBehaviour, ITutorialShowStepValidator {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public bool ShowAllowed(long stepId) {
            CheckForQuickGameEvent checkForQuickGameEvent = new();
            ScheduleEvent(checkForQuickGameEvent, EngineService.EntityStub);
            return checkForQuickGameEvent.IsQuickGame;
        }
    }
}