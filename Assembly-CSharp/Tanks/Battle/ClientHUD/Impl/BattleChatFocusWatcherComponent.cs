using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.EventSystems;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatFocusWatcherComponent : ECSBehaviour, Component, AttachToEntityListener, DetachFromEntityListener, IPointerEnterHandler, IPointerExitHandler,
        IEventSystemHandler {
        Entity entity;

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            ScheduleEvent<PointEnterToBattleChatScrollViewEvent>(entity);
        }

        public void OnPointerExit(PointerEventData eventData) {
            ScheduleEvent<PointExitFromBattleChatScrollViewEvent>(entity);
        }
    }
}