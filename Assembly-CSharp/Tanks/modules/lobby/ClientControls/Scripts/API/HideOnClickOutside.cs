using UnityEngine;
using UnityEngine.EventSystems;

namespace tanks.modules.lobby.ClientControls.Scripts.API {
    public class HideOnClickOutside : UIBehaviour, IPointerExitHandler, IPointerEnterHandler, IEventSystemHandler {
        bool hasFocus;

        void Update() {
            if (!hasFocus && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))) {
                gameObject.SetActive(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            hasFocus = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            hasFocus = false;
        }
    }
}