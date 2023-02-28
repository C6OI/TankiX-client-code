using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialArrow : MonoBehaviour {
        RectTransform arrowPositionRect;

        RectTransform thisRect;

        void Awake() {
            thisRect = GetComponent<RectTransform>();
        }

        void Update() {
            thisRect.pivot = arrowPositionRect.pivot;
            thisRect.sizeDelta = arrowPositionRect.sizeDelta;
            thisRect.position = arrowPositionRect.position;
            thisRect.rotation = arrowPositionRect.rotation;
        }

        public void Setup(RectTransform arrowPositionRect) {
            this.arrowPositionRect = arrowPositionRect;
        }
    }
}