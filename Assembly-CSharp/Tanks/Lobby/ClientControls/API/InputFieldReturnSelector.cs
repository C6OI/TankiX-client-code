using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class InputFieldReturnSelector : MonoBehaviour {
        public Selectable selectable;

        public InputField inputField;

        public InputFieldReturnSelector overridenSelector;

        protected virtual void Start() {
            if (overridenSelector != null) {
                overridenSelector.enabled = false;
            }
        }

        void LateUpdate() {
            if (CurrentInputSelected() && Input.GetKeyDown(KeyCode.Return)) {
                ExecuteEvents.Execute(selectable.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                SelectCurrentInput();
            }
        }

        protected virtual bool CurrentInputSelected() => EventSystem.current.currentSelectedGameObject == inputField.gameObject;

        protected virtual void SelectCurrentInput() {
            inputField.Select();
        }

        public bool CanNavigateToSelectable() => selectable.interactable;
    }
}