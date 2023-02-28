using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(EntityBehaviour))]
    public class RightMouseButtonClickSender : MonoBehaviour, IPointerClickHandler, IEventSystemHandler {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                EngineService.Engine.ScheduleEvent(new RightMouseButtonClickEvent(), GetComponent<EntityBehaviour>().Entity);
            }
        }
    }
}