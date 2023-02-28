using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleTooltipShowComponent : TooltipShowBehaviour {
        public override void ShowTooltip(Vector3 mousePosition) {
            tooltipShowed = true;
            Entity moduleEntity = GetComponent<ModuleCardItemUIComponent>().ModuleEntity;
            Engine engine = EngineService.Engine;
            CheckForTutorialEvent checkForTutorialEvent = new();
            engine.ScheduleEvent(checkForTutorialEvent, EngineService.EntityStub);

            if (!checkForTutorialEvent.TutorialIsActive) {
                engine.ScheduleEvent<ModuleTooltipShowEvent>(moduleEntity);
            }
        }

        public void ShowTooltip(object data) {
            Engine engine = EngineService.Engine;
            CheckForTutorialEvent checkForTutorialEvent = new();
            engine.ScheduleEvent(checkForTutorialEvent, EngineService.EntityStub);

            if (!checkForTutorialEvent.TutorialIsActive) {
                TooltipController.Instance.ShowTooltip(Input.mousePosition, data, !customContentPrefab ? null : customPrefab);
            }
        }
    }
}