using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ExpBarEndGlowImage : MonoBehaviour {
        [SerializeField] float minX;

        [SerializeField] float maxX;

        [SerializeField] UIRectClipper clipper;

        [SerializeField] GameObject icon;

        RectTransform parentRect;

        RectTransform thisRect;

        void Awake() {
            thisRect = GetComponent<RectTransform>();
            parentRect = transform.parent.GetComponent<RectTransform>();
        }

        void Update() {
            float num = parentRect.rect.width * clipper.ToX;
            bool active = num < maxX && num > minX;
            icon.SetActive(active);
            thisRect.anchoredPosition = new Vector2(num, 0f);
        }
    }
}