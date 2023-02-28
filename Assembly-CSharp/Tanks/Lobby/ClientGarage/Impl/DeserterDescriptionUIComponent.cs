using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DeserterDescriptionUIComponent : MonoBehaviour {
        public RectTransform Rect => GetComponent<RectTransform>();

        public void ShowDescription(string text) {
            Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, 50f);
            TextMeshProUGUI componentInChildren = GetComponentInChildren<TextMeshProUGUI>(true);
            componentInChildren.text = text;
            componentInChildren.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}