using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace tanks.modules.lobby.ClientControls.API {
    public class OnClickOutside : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] UnityEvent onClick;

        bool isInside;

        void Update() {
            if (!isInside && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
                onClick.Invoke();
            }
        }

        void OnDisable() {
            isInside = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            isInside = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            isInside = false;
        }
    }
}