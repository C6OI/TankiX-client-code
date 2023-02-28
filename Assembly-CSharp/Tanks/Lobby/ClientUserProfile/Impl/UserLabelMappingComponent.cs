using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserLabelMappingComponent : UserLabelBaseMappingComponent, IPointerClickHandler, IEventSystemHandler {
        public void OnPointerClick(PointerEventData eventData) {
            if (entity != null) {
                EngineService.Engine.ScheduleEvent<UserLabelClickEvent>(entity);
            }
        }
    }
}