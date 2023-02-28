using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DialogueHideComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        public BaseDialogComponent dialogComponent;

        bool pointerIn;

        void Update() {
            if (!pointerIn && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && dialogComponent != null) {
                dialogComponent.Hide();
            }
        }

        void OnDisable() {
            pointerIn = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            pointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            pointerIn = false;
        }
    }
}