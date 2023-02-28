using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ScreenBehaviour : StateMachineBehaviour {
        protected CanvasGroup GetCanvasGroup(GameObject gameObject) {
            CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null) {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            return canvasGroup;
        }
    }
}