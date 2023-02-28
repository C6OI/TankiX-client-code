using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(Selectable))]
    public class ButtonSelectionHandler : MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IEventSystemHandler {
        public void OnDeselect(BaseEventData eventData) {
            StartCoroutine(SelectAgain());
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Selectable component = GetComponent<Selectable>();

            if (component.interactable) {
                component.Select();
            }
        }

        IEnumerator SelectAgain() {
            yield return new WaitForEndOfFrame();

            GameObject newSelection = EventSystem.current.currentSelectedGameObject;

            if (newSelection == null || newSelection.GetComponent<ButtonSelectionHandler>() == null && newSelection.GetComponent<InputField>() == null &&
                newSelection.GetComponent<InputFieldComponent>() == null) {
                GetComponent<Selectable>().Select();
            }
        }
    }
}