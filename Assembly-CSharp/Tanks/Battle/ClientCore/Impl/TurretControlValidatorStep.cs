using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TurretControlValidatorStep : MonoBehaviour, ITutorialShowStepValidator {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public bool ShowAllowed(long stepId) {
            CheckUserForSpectatorEvent checkUserForSpectatorEvent = new();
            EngineService.Engine.ScheduleEvent(checkUserForSpectatorEvent, EngineService.EntityStub);
            return !checkUserForSpectatorEvent.UserIsSpectator;
        }
    }
}