using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class InputFieldReturnSelector : MonoBehaviour {
        public Selectable selectable;

        public InputField inputField;

        public InputFieldReturnSelector overridenSelector;

        void Start() {
            if (overridenSelector != null) {
                overridenSelector.enabled = false;
            }
        }

        void LateUpdate() {
            if (EventSystem.current.currentSelectedGameObject == inputField.gameObject && Input.GetKeyDown(KeyCode.Return)) {
                ExecuteEvents.Execute(selectable.gameObject,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerClickHandler);

                inputField.Select();
            }
        }

        public bool CanNavigateToSelectable() => selectable.interactable;
    }
}