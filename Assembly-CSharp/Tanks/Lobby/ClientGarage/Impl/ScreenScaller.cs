using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ScreenScaller : MonoBehaviour {
        [SerializeField] int screenHeight;

        [SerializeField] int aspectRatioX;

        [SerializeField] int aspectRatioY;

        void Update() {
            if (GetComponentInParent<Canvas>() != null) {
                GameObject gameObject = GetComponentInParent<Canvas>().gameObject;
                float height = gameObject.GetComponent<RectTransform>().rect.height;

                if (aspectRatioX / (float)aspectRatioY > Screen.width / (float)Screen.height) {
                    GetComponent<RectTransform>().localScale = Vector3.one * screenHeight / height;
                } else {
                    GetComponent<RectTransform>().localScale = Vector3.one * height / screenHeight;
                }
            }
        }
    }
}