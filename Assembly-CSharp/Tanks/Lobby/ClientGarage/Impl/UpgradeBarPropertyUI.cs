using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeBarPropertyUI : UpgradePropertyUI {
        [SerializeField] protected GameObject barContent;

        [SerializeField] protected Slider currentValueSlider;

        [SerializeField] protected Slider nextValueSlider;

        [SerializeField] protected RectTransform currentValueFill;

        [SerializeField] protected RectTransform nextValueFill;

        void Update() {
            if (!(barContent == null)) {
                nextValueFill.offsetMin = new Vector2(currentValueFill.rect.width, nextValueFill.offsetMin.y);
            }
        }

        public override void SetValue(string name, string unit, string currentValueStr) {
            base.SetValue(name, unit, currentValueStr);
            barContent.SetActive(false);
        }

        public override void SetUpgradableValue(string name, string unit, string currentValueStr, string nextValueStr, float minValue, float maxValue, float currentValue, float nextValue,
            string format) {
            base.SetUpgradableValue(name, unit, currentValueStr, nextValueStr, minValue, maxValue, currentValue, nextValue, format);
            Slider slider = currentValueSlider;
            float minValue2 = Mathf.Abs(minValue);
            nextValueSlider.minValue = minValue2;
            slider.minValue = minValue2;
            Slider slider2 = currentValueSlider;
            minValue2 = Mathf.Abs(maxValue);
            nextValueSlider.maxValue = minValue2;
            slider2.maxValue = minValue2;
            currentValueSlider.value = Mathf.Abs(currentValue);
            nextValueSlider.value = Mathf.Abs(nextValue);
        }
    }
}