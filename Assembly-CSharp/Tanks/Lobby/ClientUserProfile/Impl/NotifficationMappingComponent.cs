using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NotifficationMappingComponent : BehaviourComponent, IPointerClickHandler, AttachToEntityListener, DetachFromEntityListener, IEventSystemHandler {
        [SerializeField] bool clickAnywhere;

        Entity entity;

        void Update() {
            if (clickAnywhere && Input.GetMouseButton(0)) {
                MouseClicked();
            }
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void OnPointerClick(PointerEventData eventData) {
            MouseClicked();
        }

        void MouseClicked() {
            EngineService.Engine.ScheduleEvent<NotificationClickEvent>(entity);
            enabled = false;
        }
    }
}