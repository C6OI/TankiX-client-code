using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HPBarGlow : HUDBar {
        [SerializeField] Image fill;

        [SerializeField] Image diff;

        [SerializeField] TextMeshProUGUI hpValues;

        [SerializeField] HPBarFillEnd hpBarFillEnd;

        public float Diff { get; private set; }

        public override float CurrentValue {
            get => currentValue;
            set => Change(value - currentValue);
        }

        public override float AmountPerSegment => maxValue;

        public void Change(float delta) {
            Diff = delta;
            Diff = Clamp(currentValue + Diff) - currentValue;
            float num = Mathf.Max(currentValue + Diff, currentValue);
            float num2 = Mathf.Min(currentValue + Diff, currentValue);
            float num3 = num2 / maxValue;
            float fillAmount = num / maxValue;
            SetFillAmount(fill, num3);
            SetFillAmount(diff, fillAmount);
            hpBarFillEnd.FillAmount = fillAmount;
            Animator component = GetComponent<Animator>();

            if (component.isActiveAndEnabled) {
                component.SetFloat("Fill", num3);
                component.SetInteger("Diff", (int)Diff);
                component.SetTrigger("Start");
            }

            fill.color = !(num3 > 0.2f) ? new Color32(byte.MaxValue, 59, 59, byte.MaxValue) : new Color32(168, byte.MaxValue, 47, byte.MaxValue);
            UpdateHPValues((int)(currentValue + Mathf.Max(0f, Diff)));
        }

        void SetFillAmount(Image image, float fillAmount) {
            image.fillAmount = fillAmount;
        }

        public void ResetDiff() {
            currentValue = Clamp(currentValue + Diff);
            Diff = 0f;
            float num = currentValue / maxValue;
            SetFillAmount(fill, num);
            hpBarFillEnd.FillAmount = num;
            fill.color = !(num > 0.2f) ? new Color32(byte.MaxValue, 59, 59, byte.MaxValue) : new Color32(168, byte.MaxValue, 47, byte.MaxValue);
            Animator component = GetComponent<Animator>();

            if (component.isActiveAndEnabled) {
                component.SetInteger("Diff", 0);
                component.SetFloat("Fill", num);
            }

            UpdateHPValues((int)currentValue);
        }

        protected override void OnMaxValueChanged() {
            ResetDiff();
        }

        void UpdateHPValues(int value) {
            hpValues.text = string.Format("{0}/<size=16>{1}", value, (int)maxValue);
        }
    }
}