using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine.EventSystems;

namespace Lobby.ClientControls.API {
    public abstract class EventSystemPointerEvent : Event {
        public PointerEventData PointerEventData { get; set; }
    }
}