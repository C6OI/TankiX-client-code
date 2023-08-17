using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = UnityEngine.Event;

namespace Lobby.ClientControls.API {
    public class ConfirmButtonFlipFrontOnClickOutside : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler,
        IPointerExitHandler, Component, ComponentLifecycle {
        [SerializeField] ConfirmButtonComponent confirmButton;

        Entity entity;

        bool inside;

        void OnGUI() {
            if (!confirmButton.EnableOutsideClicking) {
                return;
            }

            if (Event.current.type == EventType.MouseUp && !inside) {
                FlipFront();
            }

            if (Event.current.type == EventType.KeyDown) {
                float axis = Input.GetAxis("Vertical");

                if (!Mathf.Approximately(axis, 0f)) {
                    FlipFront();
                }
            }
        }

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => this.entity = null;

        public void OnPointerEnter(PointerEventData eventData) => inside = true;

        public void OnPointerExit(PointerEventData eventData) => inside = false;

        void FlipFront() {
            confirmButton.FlipFront();

            if (entity != null) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine flow) {
                    flow.ScheduleEvent<ConfirmButtonClickOutsideEvent>(entity);
                });
            }
        }
    }
}