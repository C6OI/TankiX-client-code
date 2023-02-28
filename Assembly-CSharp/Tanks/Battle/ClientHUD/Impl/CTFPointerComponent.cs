using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class CTFPointerComponent : MonoBehaviour, Component {
        public RectTransform parentCanvasRect;

        public RectTransform selfRect;

        public CanvasGroup canvasGroup;

        public Text text;

        void OnDisable() {
            Hide();
        }

        public void Hide() {
            canvasGroup.alpha = 0f;
        }

        public void Show() {
            canvasGroup.alpha = 1f;
        }
    }
}