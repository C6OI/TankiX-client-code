using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class UserEnergyBarUIComponent : BehaviourComponent {
        [SerializeField] float animationSpeed = 1f;

        [SerializeField] Slider slider;

        [SerializeField] Slider subSlider;

        [SerializeField] TextMeshProUGUI energyLevel;

        long currentValue;

        float mainSliderValue;

        long maxValue;

        float subSliderValue;

        void Update() {
            LerpSliderValue(slider, mainSliderValue);
            LerpSliderValue(subSlider, subSliderValue);
        }

        void OnEnable() {
            slider.value = 0f;
            subSlider.value = 0f;
        }

        public void SetEnergyLevel(long currentValue, long maxValue) {
            this.currentValue = currentValue;
            this.maxValue = maxValue;
            mainSliderValue = currentValue / (float)maxValue;
            subSliderValue = 0f;
            SetTextValue(currentValue, maxValue);
        }

        public void SetSharedEnergyLevel(long sharedValue) {
            subSliderValue = currentValue / (float)maxValue;
            mainSliderValue = (currentValue - sharedValue) / (float)maxValue;
            SetTextValue(currentValue - sharedValue, maxValue);
        }

        public void ShowAdditionalEnergyLevel(long additionalValue) {
            SetEnergyLevel(currentValue + additionalValue, maxValue);
        }

        void SetTextValue(long value, long maxValue) {
            energyLevel.text = string.Format("{0}/{1}", value, maxValue);
        }

        void LerpSliderValue(Slider slider, float value) {
            float num = Mathf.Abs(slider.value - value);

            if (num != 0f) {
                slider.value = Mathf.Lerp(slider.value, value, Time.deltaTime * animationSpeed / num);
            }
        }
    }
}