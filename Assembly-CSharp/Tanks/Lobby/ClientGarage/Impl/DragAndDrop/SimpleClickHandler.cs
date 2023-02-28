using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl.DragAndDrop {
    public class SimpleClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler {
        public Action<GameObject> onClick;

        public void OnPointerClick(PointerEventData eventData) {
            if (onClick != null) {
                onClick(gameObject);
            }
        }
    }
}