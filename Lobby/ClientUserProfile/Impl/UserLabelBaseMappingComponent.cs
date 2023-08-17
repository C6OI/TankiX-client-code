using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.EventSystems;

namespace Lobby.ClientUserProfile.Impl {
    public class UserLabelBaseMappingComponent : BehaviourComponent, IEventSystemHandler, IPointerDownHandler,
        IPointerUpHandler, ComponentLifecycle {
        protected Entity entity;

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => this.entity = null;

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) { }
    }
}