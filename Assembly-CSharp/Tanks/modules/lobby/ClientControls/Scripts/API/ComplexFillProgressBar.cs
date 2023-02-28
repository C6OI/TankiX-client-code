using System;
using UnityEngine;
using UnityEngine.UI;

namespace tanks.modules.lobby.ClientControls.Scripts.API {
    public class ComplexFillProgressBar : MonoBehaviour {
        const string INVALID_PROGRESS_FORMAT = "Incorrect ProgressValue {0}. The available ProgressValue's range is [0,1]";

        [SerializeField] Image maskImage;

        RectTransform maskImageRectTransform;

        RectTransform parentRectTransform;

        float val;

        public float ProgressValue {
            get => val;
            set {
                if (value < 0f || value > 1f) {
                    throw new ArgumentException(string.Format("Incorrect ProgressValue {0}. The available ProgressValue's range is [0,1]", value));
                }

                val = value;
                Vector2 offsetMax = maskImageRectTransform.offsetMax;
                offsetMax.x = (val - 1f) * parentRectTransform.rect.width;
                maskImageRectTransform.offsetMax = offsetMax;
            }
        }

        public void Awake() {
            if (maskImage == null) {
                Mask componentInChildren = gameObject.GetComponentInChildren<Mask>();
                maskImage = componentInChildren.gameObject.GetComponent<Image>();
            }

            maskImageRectTransform = maskImage.GetComponent<RectTransform>();
            parentRectTransform = maskImage.transform.parent.GetComponent<RectTransform>();
        }
    }
}