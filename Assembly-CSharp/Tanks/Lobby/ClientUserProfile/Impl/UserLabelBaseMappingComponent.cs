using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserLabelBaseMappingComponent : BehaviourComponent, IPointerDownHandler, IPointerUpHandler, AttachToEntityListener, DetachFromEntityListener, IEventSystemHandler {
        protected Entity entity;

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) { }
    }
}