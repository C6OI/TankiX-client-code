using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientNotifications.Impl {
    class ItemNotificationMappingComponent : BehaviourComponent, IPointerClickHandler, IEventSystemHandler,
        ComponentLifecycle {
        Entity entity;

        void Update() {
            if (InputMapping.Cancel) {
                CloseNotification();
            }
        }

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => entity = null;

        public void OnPointerClick(PointerEventData eventData) => CloseNotification();

        void CloseNotification() {
            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
                e.ScheduleEvent<ItemNotificationCloseEvent>(entity);
            });

            enabled = false;
        }
    }
}