using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    public class EventSystemProviderComponent : MonoBehaviour, IPointerClickHandler, IEventSystemHandler,
        IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, Component {
        EntityBehaviour entityBehaviour;

        [Inject] public static EngineService EngineService { get; set; }

        void Awake() => entityBehaviour = GetComponent<EntityBehaviour>();

        public void OnBeginDrag(PointerEventData eventData) => ExecuteInFlow<EventSystemOnBeginDragEvent>(eventData);

        public void OnDrag(PointerEventData eventData) => ExecuteInFlow<EventSystemOnDragEvent>(eventData);

        public void OnEndDrag(PointerEventData eventData) => ExecuteInFlow<EventSystemOnEndDragEvent>(eventData);

        public void OnPointerClick(PointerEventData eventData) => ExecuteInFlow<EventSystemOnPointerClickEvent>(eventData);

        public void OnPointerDown(PointerEventData eventData) => ExecuteInFlow<EventSystemOnPointerDownEvent>(eventData);

        void ExecuteInFlow<T>(PointerEventData eventData) where T : EventSystemPointerEvent, new() {
            T eventInstance = new();
            eventInstance.PointerEventData = eventData;

            ExecuteInFlow(delegate(Engine engine) {
                engine.ScheduleEvent(eventInstance, entityBehaviour.Entity);
            });
        }

        void ExecuteInFlow(Consumer<Engine> consumer) {
            if (Flow.CurrentFlowExist) {
                Flow.Current.ScheduleWith(consumer);
            } else {
                EngineService.ExecuteInFlow(consumer);
            }
        }
    }
}