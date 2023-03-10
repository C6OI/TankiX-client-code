using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    public class CursorSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        static GameObject overObject;

        public CursorType cursorType;

        void OnDisable() {
            if (overObject == gameObject) {
                overObject = null;
                Cursors.SwitchToDefaultCursor();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (cursorType == CursorType.HAND && gameObject.IsInteractable() || cursorType != 0) {
                Cursors.SwitchToCursor(cursorType);
                overObject = gameObject;
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            Cursors.SwitchToDefaultCursor();
        }
    }
}