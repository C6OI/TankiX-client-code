using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public static class ControlUtils {
        public static bool IsInteractable(this GameObject gameObject) {
            Selectable component = gameObject.GetComponent<Selectable>();
            CanvasGroup component2 = gameObject.GetComponent<CanvasGroup>();

            if (component2 != null && !component2.interactable) {
                return false;
            }

            return component != null && component.enabled && component.interactable;
        }

        public static void SetInteractable(this GameObject gameObject, bool interactable) =>
            gameObject.GetComponent<Selectable>().interactable = interactable;
    }
}