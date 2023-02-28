using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class ModuleResearchStepValidator : ECSBehaviour, ITutorialShowStepValidator {
        [SerializeField] long moduleId;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public bool ShowAllowed(long stepId) {
            CheckModuleForResearchEvent checkModuleForResearchEvent = new();
            checkModuleForResearchEvent.StepId = stepId;
            checkModuleForResearchEvent.ItemId = moduleId;
            CheckModuleForResearchEvent checkModuleForResearchEvent2 = checkModuleForResearchEvent;
            ScheduleEvent(checkModuleForResearchEvent2, EngineService.EntityStub);
            return checkModuleForResearchEvent2.ResearchAvailable;
        }
    }
}