using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class ProgressBar : MonoBehaviour {
        const string INVALID_PROGRESS_FORMAT = "Incorrect ProgressValue {0}. The available ProgressValue's range is [0,1]";

        [SerializeField] Image maskImage;

        public float ProgressValue {
            get => maskImage.fillAmount;
            set {
                if (value < 0f || value > 1f) {
                    throw new ArgumentException(
                        string.Format("Incorrect ProgressValue {0}. The available ProgressValue's range is [0,1]", value));
                }

                maskImage.fillAmount = value;
            }
        }

        public void Awake() {
            if (maskImage == null) {
                Mask componentInChildren = gameObject.GetComponentInChildren<Mask>();
                maskImage = componentInChildren.gameObject.GetComponent<Image>();
            }
        }
    }
}