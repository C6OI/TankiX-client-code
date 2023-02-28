using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class ChangeTurretTutorialStepValidator : ECSBehaviour, ITutorialShowStepValidator {
        [SerializeField] long itemId;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public bool ShowAllowed(long stepId) {
            GetChangeTurretTutorialValidationDataEvent getChangeTurretTutorialValidationDataEvent = new(stepId, itemId);
            ScheduleEvent(getChangeTurretTutorialValidationDataEvent, EngineService.EntityStub);
            return getChangeTurretTutorialValidationDataEvent.BattlesCount > 0 && !getChangeTurretTutorialValidationDataEvent.TutorialItemAlreadyMounted;
        }
    }
}