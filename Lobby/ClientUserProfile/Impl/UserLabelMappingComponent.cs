using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.EventSystems;

namespace Lobby.ClientUserProfile.Impl {
    public class UserLabelMappingComponent : UserLabelBaseMappingComponent, IPointerClickHandler, IEventSystemHandler {
        public void OnPointerClick(PointerEventData eventData) {
            if (entity != null) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine flow) {
                    flow.ScheduleEvent<UserLabelClickEvent>(entity);
                });
            }
        }
    }
}