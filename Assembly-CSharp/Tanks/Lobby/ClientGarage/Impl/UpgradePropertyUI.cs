using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradePropertyUI : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI valueLabel;

        [SerializeField] protected TextMeshProUGUI nextValueLabel;

        [SerializeField] protected new TextMeshProUGUI name;

        [SerializeField] protected GameObject arrow;

        int level;

        public virtual void SetValue(string name, string unit, float currentValue, string format) {
            SetValue(name, unit, FormatValue(currentValue, format));
        }

        public virtual void SetUpgradableValue(string name, string unit, float minValue, float maxValue, float currentValue, float nextValue, string format) {
            SetUpgradableValue(name, unit, FormatValue(currentValue, format), FormatValue(nextValue, format), currentValue, nextValue, minValue, maxValue, format);
        }

        public virtual void SetValue(string name, string unit, string currentValueStr) {
            this.name.text = name;
            valueLabel.text = currentValueStr + " " + unit;
            Image component = arrow.GetComponent<Image>();
            Color clear = Color.clear;
            nextValueLabel.color = clear;
            component.color = clear;
        }

        public virtual void SetUpgradableValue(string name, string unit, string currentValueStr, string nextValueStr, float currentValue, float nextValue, float minValue, float maxValue,
            string format) {
            this.name.text = name;
            valueLabel.text = currentValueStr + " " + unit;
            nextValueLabel.text = nextValueStr + " " + unit;
        }

        string FormatValue(float value, string format) {
            string format2 = "{0:" + format + "}";
            float num = Mathf.RoundToInt(value * 100f) / 100f;
            return string.Format(format2, value);
        }

        public void DisableNextValueAndArrow() {
            nextValueLabel.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
        }
    }
}