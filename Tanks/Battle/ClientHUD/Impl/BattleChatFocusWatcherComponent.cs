using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatFocusWatcherComponent : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler,
        IPointerExitHandler, Component, ComponentLifecycle {
        Entity entity;

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => this.entity = entity;

        public void OnPointerEnter(PointerEventData eventData) => ClientUnityIntegrationUtils.ExecuteInFlow(
            delegate(Engine flow) {
                flow.ScheduleEvent<PointEnterToBattleChatScrollViewEvent>(entity);
            });

        public void OnPointerExit(PointerEventData eventData) => ClientUnityIntegrationUtils.ExecuteInFlow(
            delegate(Engine flow) {
                flow.ScheduleEvent<PointExitFromBattleChatScrollViewEvent>(entity);
            });
    }
}