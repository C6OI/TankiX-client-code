using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Button))]
    public class ButtonSelectionHandler : MonoBehaviour, IDeselectHandler, IEventSystemHandler, IPointerEnterHandler {
        public void OnDeselect(BaseEventData eventData) => StartCoroutine(SelectAgain());

        public void OnPointerEnter(PointerEventData eventData) {
            Button component = GetComponent<Button>();

            if (component.interactable) {
                component.Select();
            }
        }

        IEnumerator SelectAgain() {
            yield return new WaitForEndOfFrame();

            GameObject newSelection = EventSystem.current.currentSelectedGameObject;

            if (newSelection == null ||
                newSelection.GetComponent<ButtonSelectionHandler>() == null &&
                newSelection.GetComponent<InputField>() == null) {
                GetComponent<Button>().Select();
            }
        }
    }
}