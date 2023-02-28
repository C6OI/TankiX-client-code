using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class ModuleUpgradeStepValidator : ECSBehaviour, ITutorialShowStepValidator {
        [SerializeField] long moduleId;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public bool ShowAllowed(long stepId) {
            CheckModuleForUpgradeEvent checkModuleForUpgradeEvent = new();
            checkModuleForUpgradeEvent.StepId = stepId;
            checkModuleForUpgradeEvent.ItemId = moduleId;
            CheckModuleForUpgradeEvent checkModuleForUpgradeEvent2 = checkModuleForUpgradeEvent;
            ScheduleEvent(checkModuleForUpgradeEvent2, EngineService.EntityStub);
            return checkModuleForUpgradeEvent2.UpgradeAvailable;
        }
    }
}