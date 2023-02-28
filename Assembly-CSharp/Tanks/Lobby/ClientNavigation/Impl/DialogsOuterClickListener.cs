using System;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class DialogsOuterClickListener : UIBehaviour, IPointerClickHandler, IEventSystemHandler {
        public Action<PointerEventData> ClickAction { get; set; }

        public void OnPointerClick(PointerEventData eventData) {
            ClickAction(eventData);
        }
    }
}