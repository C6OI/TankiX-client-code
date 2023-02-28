using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleTooltipContent : MonoBehaviour, ITooltipContent {
        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TextMeshProUGUI description;

        [SerializeField] TextMeshProUGUI upgradeLevel;

        [SerializeField] LocalizedField upgradeLevelLocalization;

        [SerializeField] TextMeshProUGUI currentLevel;

        [SerializeField] TextMeshProUGUI nextLevel;

        int UpgradeLevel {
            set {
                upgradeLevel.gameObject.SetActive(value > 0);
                upgradeLevel.text = upgradeLevelLocalization.Value + " " + value;
            }
        }

        int CurrentLevel {
            set {
                currentLevel.gameObject.SetActive(value != -1);
                currentLevel.text = upgradeLevelLocalization.Value + " " + value;
            }
        }

        int NextLevel {
            set {
                nextLevel.gameObject.SetActive(value != -1);
                nextLevel.text = upgradeLevelLocalization.Value + " " + value;
            }
        }

        public void Init(object data) {
            ModuleTooltipData moduleTooltipData = data as ModuleTooltipData;
            title.text = moduleTooltipData.name;
            description.text = moduleTooltipData.desc;
            UpgradeLevel = moduleTooltipData.upgradeLevel + 1;
            ModulesPropertiesUIComponent component = GetComponent<ModulesPropertiesUIComponent>();

            if (moduleTooltipData.upgradeLevel != -1 && moduleTooltipData.upgradeLevel != moduleTooltipData.maxLevel) {
                CurrentLevel = moduleTooltipData.upgradeLevel + 1;
                NextLevel = moduleTooltipData.upgradeLevel + 2;
            } else {
                int num2 = NextLevel = -1;
                CurrentLevel = num2;
            }

            for (int i = 0; i < moduleTooltipData.properties.Count; i++) {
                ModuleVisualProperty moduleVisualProperty = moduleTooltipData.properties[i];

                if (moduleVisualProperty.Upgradable && moduleTooltipData.upgradeLevel != moduleTooltipData.maxLevel && moduleTooltipData.upgradeLevel != -1) {
                    float minValue = 0f;
                    float maxValue = moduleVisualProperty.CalculateModuleEffectPropertyValue(moduleTooltipData.maxLevel, moduleTooltipData.maxLevel);

                    float currentValue = moduleTooltipData.upgradeLevel == -1 ? 0f
                                             : moduleVisualProperty.CalculateModuleEffectPropertyValue(moduleTooltipData.upgradeLevel, moduleTooltipData.maxLevel);

                    float nextValue = moduleVisualProperty.CalculateModuleEffectPropertyValue(moduleTooltipData.upgradeLevel + 1, moduleTooltipData.maxLevel);
                    component.AddProperty(moduleVisualProperty.Name, moduleVisualProperty.Unit, minValue, maxValue, currentValue, nextValue, moduleVisualProperty.Format);
                } else if (moduleTooltipData.upgradeLevel == -1) {
                    float currentValue2 = moduleVisualProperty.CalculateModuleEffectPropertyValue(0, moduleTooltipData.maxLevel);
                    component.AddProperty(moduleVisualProperty.Name, moduleVisualProperty.Unit, currentValue2, moduleVisualProperty.Format);
                } else {
                    float currentValue3 = moduleVisualProperty.CalculateModuleEffectPropertyValue(moduleTooltipData.upgradeLevel, moduleTooltipData.maxLevel);
                    component.AddProperty(moduleVisualProperty.Name, moduleVisualProperty.Unit, currentValue3, moduleVisualProperty.Format);
                }
            }
        }
    }
}