using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AnimatedPropertyProgressBar : MonoBehaviour {
        [SerializeField] Image slider;

        bool canStart;

        float finalValue;

        void Start() {
            finalValue = slider.fillAmount;
            slider.fillAmount = 0f;
            canStart = true;
        }

        void Update() {
            if (canStart) {
                slider.fillAmount = Mathf.Lerp(slider.fillAmount, finalValue, 0.1f);

                if (slider.fillAmount == finalValue) {
                    canStart = false;
                }
            }
        }

        void OnDisable() {
            slider.fillAmount = 0f;
        }
    }
}