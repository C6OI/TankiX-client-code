using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemAttributesComponent : BehaviourComponent, AttachToEntityListener {
        [SerializeField] GameObject proficiency;

        [SerializeField] GameObject experience;

        [SerializeField] GameObject upgrade;

        [SerializeField] Text upgradeLevelValue;

        [SerializeField] Text nextUpgradeLevelValue;

        [SerializeField] ProgressBar upgradeLevelProgress;

        [SerializeField] Text proficiencyLevelValue;

        [SerializeField] ProgressBar proficiencyLevelProgress;

        [SerializeField] Text experienceValue;

        [SerializeField] Text maxExperienceValue;

        [SerializeField] ProgressBar remainingExperienceProgress;

        [SerializeField] GameObject nextUpgrade;

        [SerializeField] Image upgradeGlow;

        [SerializeField] Image colorIcon;

        [SerializeField] bool showNextUpgradeValue;

        public Color nextValueOverUpgradeColor;

        public Color nextValueUpgradeColor;

        bool runtimeShowNextUpgradeValue;

        Color upgradeColor;

        long upgradeLevel;

        public bool ShowNextUpgradeValue {
            get => showNextUpgradeValue && runtimeShowNextUpgradeValue;
            set {
                runtimeShowNextUpgradeValue = value;
                nextUpgrade.SetActive(showNextUpgradeValue && runtimeShowNextUpgradeValue);
            }
        }

        void Awake() {
            nextUpgrade.SetActive(showNextUpgradeValue);
            RectTransform component = upgradeGlow.GetComponent<RectTransform>();
            Vector2 sizeDelta = component.sizeDelta;
            sizeDelta.x *= 100f / UpgradablePropertiesUtils.MAX_LEVEL;
            component.sizeDelta = sizeDelta;
        }

        public void AttachedToEntity(Entity entity) {
            runtimeShowNextUpgradeValue = true;
            upgradeLevelValue.text = string.Empty;
            proficiencyLevelValue.text = string.Empty;
            upgradeLevel = 0L;
            remainingExperienceProgress.ProgressValue = 1f;
        }

        public void AnimateUpgrade(long level) {
            GetComponent<Animator>().SetTrigger("Upgrade");
            upgradeLevel = level;
            UpdateUpgradeColor();
            RectTransform component = upgradeGlow.GetComponent<RectTransform>();
            component.anchorMax = new Vector2(upgradeLevelProgress.ProgressValue, component.anchorMax.y);
            component.anchorMin = new Vector2(upgradeLevelProgress.ProgressValue, component.anchorMin.y);
        }

        public void SetUpgradeLevel(long level) {
            upgradeLevel = level;
            UpdateUpgradeColor();
            ApplyValues();
            ApplyUpdateColor();
        }

        void ApplyValues() {
            upgradeLevelValue.text = upgradeLevel.ToString();
            ShowNextUpgradeValue = ShowNextUpgradeValue && upgradeLevel < UpgradablePropertiesUtils.MAX_LEVEL;
            nextUpgradeLevelValue.text = (upgradeLevel + 1).ToString();
            upgradeLevelProgress.ProgressValue = upgradeLevel / (float)UpgradablePropertiesUtils.MAX_LEVEL;
            nextUpgradeLevelValue.color = upgradeColor;
        }

        void OnFinishAnimation() {
            ApplyUpdateColor();
        }

        public void SetProficiencyLevel(int level) {
            proficiencyLevelValue.text = level.ToString();
            proficiencyLevelProgress.ProgressValue = level / (float)UpgradablePropertiesUtils.MAX_LEVEL;
            UpdateUpgradeColor();
            ApplyUpdateColor();
        }

        public void SetExperience(int exp, int initLevelExp, int finalLevelExp) {
            experienceValue.text = (finalLevelExp - exp - initLevelExp).ToString();
            maxExperienceValue.text = (finalLevelExp - initLevelExp).ToString();
            remainingExperienceProgress.ProgressValue = (finalLevelExp - exp - initLevelExp) / (float)(finalLevelExp - initLevelExp);
        }

        public void Hide() {
            ChangeChildrenVisibility(false);
        }

        public void HideExperience() {
            experience.SetActive(false);
        }

        public void Show() {
            ChangeChildrenVisibility(true);
        }

        void ChangeChildrenVisibility(bool visible) {
            proficiency.SetActive(visible);
            experience.SetActive(visible && remainingExperienceProgress.ProgressValue < 1f && proficiencyLevelProgress.ProgressValue < 1f);
            upgrade.SetActive(visible);
        }

        void UpdateUpgradeColor() {
            if (showNextUpgradeValue && upgradeLevel >= 0 && !string.IsNullOrEmpty(proficiencyLevelValue.text)) {
                upgradeColor = nextValueUpgradeColor;
            }
        }

        void ApplyUpdateColor() {
            nextUpgradeLevelValue.color = upgradeColor;
            upgradeGlow.color = upgradeColor;
            colorIcon.color = upgradeColor;
        }
    }
}