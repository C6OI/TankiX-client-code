using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialMask : MonoBehaviour {
        RectTransform targetRect;

        RectTransform thisRect;

        void Awake() {
            thisRect = GetComponent<RectTransform>();
        }

        void Update() {
            thisRect.pivot = targetRect.pivot;
            thisRect.position = targetRect.position;
            thisRect.sizeDelta = new Vector2(targetRect.rect.width, targetRect.rect.height);
        }

        public void Init(RectTransform targetRect) {
            this.targetRect = targetRect;
        }
    }
}