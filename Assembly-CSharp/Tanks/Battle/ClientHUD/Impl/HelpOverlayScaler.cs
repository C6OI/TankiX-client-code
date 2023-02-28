using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HelpOverlayScaler : MonoBehaviour {
        public int resolutionXScaleSize;

        public GameObject[] scaledObjects;

        public Vector3 scaleFactor;

        void Start() {
            int width = Screen.width;

            if (width <= resolutionXScaleSize) {
                GameObject[] array = scaledObjects;

                foreach (GameObject gameObject in array) {
                    gameObject.transform.localScale = scaleFactor;
                }
            }
        }
    }
}