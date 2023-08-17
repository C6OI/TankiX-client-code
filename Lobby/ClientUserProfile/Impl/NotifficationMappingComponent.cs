using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.EventSystems;

namespace Lobby.ClientUserProfile.Impl {
    public class NotifficationMappingComponent : BehaviourComponent, IPointerClickHandler, IEventSystemHandler,
        ComponentLifecycle {
        Entity entity;

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => entity = null;

        public void OnPointerClick(PointerEventData eventData) {
            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
                e.ScheduleEvent<NotificationClickEvent>(entity);
            });

            enabled = false;
        }
    }
}