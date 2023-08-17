using System.Globalization;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientLocale.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    [RequireComponent(typeof(Animator))]
    public class PropertyUIComponent : MonoBehaviour, Component, ComponentLifecycle {
        [SerializeField] int precision;

        [SerializeField] Text currentValue;

        [SerializeField] Image arrowUpgrade;

        [SerializeField] Text nextValue;

        [SerializeField] Image stroke;

        CultureInfo culture;

        string current;

        string next;

        bool showNextUpgradeValue;

        Color upgradeColor;

        bool valuableUpgrade;

        public bool ShowNextUpgradeValue {
            get => showNextUpgradeValue;
            set {
                showNextUpgradeValue = value;
                ApplyVisibility();
            }
        }

        public void Awake() => culture = CultureInfo.CreateSpecificCulture(LocaleUtils.GetSavedLocaleCode());

        public void AttachToEntity(Entity entity) {
            nextValue.text = string.Empty;
            currentValue.text = string.Empty;
            next = string.Empty;
            current = string.Empty;
        }

        public void DetachFromEntity(Entity entity) { }

        public void SetNextValueColor(Color color) => upgradeColor = color;

        public void SetValue(float value) => current = value.ToString("N" + precision, culture);

        public void SetNextValue(float value) {
            next = value.ToString("N" + precision, culture);
            ApplyIfValuableUpgrade();
        }

        public void SetValue(float from, float to) =>
            current = from.ToString("N" + precision, culture) + "-" + to.ToString("N" + precision, culture);

        public void SetNextValue(float from, float to) {
            next = from.ToString("N" + precision, culture) + "-" + to.ToString("N" + precision, culture);
            ApplyIfValuableUpgrade();
        }

        void ApplyIfValuableUpgrade() {
            if (!string.IsNullOrEmpty(next) && !string.IsNullOrEmpty(current)) {
                valuableUpgrade = currentValue.text != current;

                if (string.IsNullOrEmpty(currentValue.text)) {
                    currentValue.text = current;
                    nextValue.text = next;
                    nextValue.color = upgradeColor;
                    stroke.color = upgradeColor;
                } else if (valuableUpgrade) {
                    GetComponent<Animator>().SetTrigger("Glow");
                } else {
                    currentValue.text = current;
                    nextValue.text = next;
                    nextValue.color = upgradeColor;
                    stroke.color = upgradeColor;
                }

                ApplyVisibility();
            }
        }

        void ApplyValues() {
            currentValue.text = current;
            nextValue.text = next;
            nextValue.color = upgradeColor;
            ApplyVisibility();
        }

        void OnFinishAnimation() => stroke.color = upgradeColor;

        void ApplyVisibility() {
            bool flag = currentValue.text != nextValue.text;
            nextValue.gameObject.SetActive(showNextUpgradeValue && flag);
            arrowUpgrade.gameObject.SetActive(showNextUpgradeValue && flag);
        }
    }
}