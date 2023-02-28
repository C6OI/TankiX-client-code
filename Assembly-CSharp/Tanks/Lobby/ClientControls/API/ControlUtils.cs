using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using UnityEngine.UI;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientControls.API {
    public static class ControlUtils {
        [Inject] public static EngineService EngineService { get; set; }

        public static bool IsInteractable(this GameObject gameObject) {
            Selectable component = gameObject.GetComponent<Selectable>();
            CanvasGroup component2 = gameObject.GetComponent<CanvasGroup>();

            if (component2 != null && !component2.interactable) {
                return false;
            }

            return component != null && component.enabled && component.interactable;
        }

        public static void SetInteractable(this GameObject gameObject, bool interactable) {
            gameObject.GetComponent<Selectable>().interactable = interactable;
        }

        public static T SendEvent<T>(this MonoBehaviour behaviour, Entity entity = null) where T : Event, new() => SendEvent(behaviour, new T(), entity);

        public static T SendEvent<T>(this MonoBehaviour behaviour, T evt, Entity entity = null) where T : Event {
            if (EngineService == null) {
                return null;
            }

            if (entity == null) {
                entity = ((EngineServiceInternal)EngineService).EntityStub;
            }

            EngineService.Engine.ScheduleEvent(evt, entity);
            return evt;
        }
    }
}