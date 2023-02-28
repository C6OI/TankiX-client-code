using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class VisualProperty {
        public VisualProperty() {
            Format = "0";
            Unit = string.Empty;
        }

        public string Name { get; set; }

        public float InitialValue { get; set; }

        public float FinalValue { get; set; }

        public float InitialAdditionValue { get; set; }

        public float FinalAdditionValue { get; set; }

        public string Format { get; set; }

        public string Unit { get; set; }

        public string GetFormatedValue(float coef) {
            string text = GetValue(coef).ToString(Format);

            if (InitialAdditionValue >= InitialValue) {
                text = text + " - " + GetAdditionalValue(coef).ToString(Format);
            }

            return text;
        }

        public float GetValue(float coef) => InitialValue + (FinalValue - InitialValue) * coef;

        public float GetAdditionalValue(float coef) => InitialAdditionValue + (FinalAdditionValue - InitialAdditionValue) * coef;

        public float GetProgress(int level) {
            float num = InitialValue + Mathf.Abs(FinalValue - InitialValue);
            int num2 = level / UpgradablePropertiesUtils.MAX_LEVEL;
            return Mathf.Lerp(InitialValue, num, num2) / num;
        }
    }
}