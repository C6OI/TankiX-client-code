using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class DialogsOuterClickEvent : Event {
        public PointerEventData EventData { get; set; }
    }
}