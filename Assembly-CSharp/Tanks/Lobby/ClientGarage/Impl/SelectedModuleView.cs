using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SelectedModuleView : MonoBehaviour {
        [SerializeField] TextMeshProUGUI moduleName;

        [SerializeField] GameObject property;

        [SerializeField] Transform upgrade;

        [SerializeField] public GameObject ResearchButton;

        [SerializeField] public GameObject UpgradeCRYButton;

        [SerializeField] GameObject UpgradeXCRYButton;

        [SerializeField] GameObject BuyBlueprints;

        [SerializeField] string damageIcon;

        [SerializeField] string armorIcon;

        [SerializeField] LocalizedField buyCRY;

        [SerializeField] LocalizedField buyXCRY;

        [SerializeField] LocalizedField bonusDamage;

        [SerializeField] LocalizedField bonusArmor;

        [SerializeField] LocalizedField localizeLVL;

        [SerializeField] TextMeshProUGUI notEnoughText;

        [SerializeField] LocalizedField notEnoughActivate;

        [SerializeField] LocalizedField notEnoughUpgrade;

        NewModulesScreenSystem.SelfUserMoneyNode money;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public void UpdateView(ModuleItem moduleItem, List<List<int>> level2PowerByTier, TankPartItem tank, TankPartItem weapon) {
            IEnumerator enumerator = upgrade.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    Destroy(transform.gameObject);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }

            if (moduleItem != null) {
                long level = moduleItem.Level;
                int maxLevel = moduleItem.MaxLevel;

                if (moduleItem.UserItem == null) {
                    moduleName.text = string.Format("{0}", moduleItem.Name);
                } else {
                    moduleName.text = string.Format("{0} <color=#838383FF>({1} {2})", moduleItem.Name, localizeLVL.Value, moduleItem.Level + 1);
                }

                ShowDamageBonus(moduleItem, maxLevel, level, level2PowerByTier, tank, weapon);

                for (int i = 0; i < moduleItem.properties.Count; i++) {
                    GameObject gameObject = Instantiate(property, upgrade);
                    gameObject.SetActive(true);
                    ModulePropertyView component = gameObject.GetComponent<ModulePropertyView>();
                    ModuleVisualProperty moduleVisualProperty = moduleItem.properties[i];
                    component.SpriteUid = moduleVisualProperty.SpriteUID;
                    component.PropertyName = moduleVisualProperty.Name;
                    component.Units = moduleVisualProperty.Unit;
                    component.Format = moduleVisualProperty.Format;

                    if (moduleVisualProperty.MaxAndMin) {
                        if (moduleItem.UserItem == null) {
                            component.CurrentParamString = moduleVisualProperty.MaxAndMinString[level];
                        } else if (level == maxLevel) {
                            component.CurrentParamString = moduleVisualProperty.MaxAndMinString[level];
                        } else {
                            component.CurrentParamString = moduleVisualProperty.MaxAndMinString[level];
                            component.NextParamString = moduleVisualProperty.MaxAndMinString[level + 1];
                        }
                    } else if (moduleItem.UserItem == null) {
                        float currentParam = moduleVisualProperty.CalculateModuleEffectPropertyValue(0, maxLevel);
                        float maxParam = moduleVisualProperty.CalculateModuleEffectPropertyValue(maxLevel, maxLevel);
                        component.CurrentParam = currentParam;
                        component.MaxParam = maxParam;
                    } else if (level == maxLevel) {
                        float maxParam2 = component.CurrentParam = moduleVisualProperty.CalculateModuleEffectPropertyValue(maxLevel, maxLevel);
                        component.MaxParam = maxParam2;
                    } else {
                        float maxParam3 = moduleVisualProperty.CalculateModuleEffectPropertyValue(maxLevel, maxLevel);
                        float currentParam2 = level == -1 ? 0f : moduleVisualProperty.CalculateModuleEffectPropertyValue((int)level, maxLevel);
                        float nextParam = moduleVisualProperty.CalculateModuleEffectPropertyValue((int)level + 1, maxLevel);
                        component.CurrentParam = currentParam2;
                        component.NextParam = nextParam;
                        component.MaxParam = maxParam3;
                    }

                    component.ProgressBar = moduleItem.properties[i].ProgressBar;
                }
            } else {
                moduleName.text = null;
            }

            ShowButton(moduleItem);
        }

        public void InitMoney(NewModulesScreenSystem.SelfUserMoneyNode money) {
            this.money = money;
        }

        public void ShowButton(ModuleItem item) {
            BuyBlueprints.SetActive(false);
            ResearchButton.SetActive(false);
            UpgradeCRYButton.SetActive(false);
            UpgradeXCRYButton.SetActive(false);

            if (item == null) {
                return;
            }

            if (item.UserItem == null && item.CraftPrice.Cards <= item.UserCardCount) {
                ResearchButton.SetActive(true);
                ResearchButton.GetComponent<ResearchModuleButtonComponent>().TitleTextActivate = string.Format("{0}", item.CraftPrice.Cards);
            } else if (item.UserItem == null && item.CraftPrice.Cards > item.UserCardCount) {
                notEnoughText.text = string.Format("{0}", notEnoughActivate.Value);
                BuyBlueprints.SetActive(true);
            } else if (item.UserItem != null && item.UpgradePrice <= item.UserCardCount && item.Level != item.MaxLevel) {
                UpgradeCRYButton.SetActive(true);
                UpgradeXCRYButton.SetActive(true);

                if (money.userMoney.Money >= item.UpgradePriceCRY) {
                    UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().TitleTextUpgrade = string.Format("{0}", item.UpgradePriceCRY + " <sprite=8>");
                    UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().NotEnoughTextEnable = false;
                } else {
                    UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().BuyCrystal = string.Format("{0}", buyCRY.Value);
                    UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().NotEnoughText = item.UpgradePriceCRY - money.userMoney.Money;
                    UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().NotEnoughTextEnable = true;
                }

                if (money.userXCrystals.Money >= item.UpgradePriceXCRY) {
                    UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().TitleTextUpgrade = string.Format("{0}", item.UpgradePriceXCRY + " <sprite=9>");
                    UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().NotEnoughTextEnable = false;
                } else {
                    UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().BuyCrystal = string.Format("{0}", buyXCRY.Value);
                    UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().NotEnoughText = item.UpgradePriceXCRY - money.userXCrystals.Money;
                    UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().NotEnoughTextEnable = true;
                }
            } else if (item.UserItem != null && item.UpgradePrice > item.UserCardCount) {
                notEnoughText.text = string.Format("{0}", notEnoughUpgrade.Value);
                BuyBlueprints.SetActive(true);
            }
        }

        void ShowDamageBonus(ModuleItem item, long max, long current, List<List<int>> level2PowerByTier, TankPartItem tank, TankPartItem weapon) {
            int num = CalculateMaximumPercentSum(level2PowerByTier, 3);

            if (num >= 0) {
                TankPartItem tankPartItem = item.TankPartModuleType != 0 ? weapon : tank;
                VisualProperty visualProperty = tankPartItem.Properties[0];
                GameObject gameObject = Instantiate(property, upgrade);
                gameObject.SetActive(true);
                ModulePropertyView component = gameObject.GetComponent<ModulePropertyView>();
                component.PropertyName = visualProperty.Name;

                if (item.TankPartModuleType == TankPartModuleType.TANK) {
                    component.PropertyName = bonusArmor;
                    component.SpriteUid = armorIcon;
                } else {
                    component.PropertyName = bonusDamage;
                    component.SpriteUid = damageIcon;
                }

                int tierNumber = item.TierNumber;

                if (item.UserItem == null) {
                    List<int[]> list = new();

                    list.Add(new int[2] {
                        tierNumber,
                        (int)current
                    });

                    List<int[]> list2 = new();

                    list2.Add(new int[2] {
                        tierNumber,
                        (int)max
                    });

                    component.CurrentParam = visualProperty.GetValue(CalculateUpgradeCoeff(list, 3, level2PowerByTier)) - visualProperty.InitialValue;
                    component.MaxParam = visualProperty.GetValue(CalculateUpgradeCoeff(list2, 3, level2PowerByTier)) - visualProperty.InitialValue;
                } else if (current == max) {
                    List<int[]> list3 = new();

                    list3.Add(new int[2] {
                        tierNumber,
                        (int)max
                    });

                    float value = visualProperty.GetValue(CalculateUpgradeCoeff(list3, 3, level2PowerByTier));
                    component.CurrentParam = value - visualProperty.InitialValue;
                    component.MaxParam = value - visualProperty.InitialValue;
                } else {
                    List<int[]> list4 = new();

                    list4.Add(new int[2] {
                        tierNumber,
                        (int)current
                    });

                    List<int[]> list5 = new();

                    list5.Add(new int[2] {
                        tierNumber,
                        (int)current + 1
                    });

                    List<int[]> list6 = new();

                    list6.Add(new int[2] {
                        tierNumber,
                        (int)max
                    });

                    component.CurrentParam = visualProperty.GetValue(CalculateUpgradeCoeff(list4, 3, level2PowerByTier)) - visualProperty.InitialValue;
                    component.NextParam = visualProperty.GetValue(CalculateUpgradeCoeff(list5, 3, level2PowerByTier)) - visualProperty.InitialValue;
                    component.MaxParam = visualProperty.GetValue(CalculateUpgradeCoeff(list6, 3, level2PowerByTier)) - visualProperty.InitialValue;
                }

                component.ProgressBar = true;
            }
        }

        float CalculateUpgradeCoeff(List<int[]> modulesParams, int slotCount, List<List<int>> level2PowerByTier) {
            int num = CollectPercentSum(modulesParams, level2PowerByTier);
            int num2 = CalculateMaximumPercentSum(level2PowerByTier, slotCount);
            return num / (float)num2;
        }

        int CalculateMaximumPercentSum(List<List<int>> level2PowerByTier, int slotCount) {
            int index = level2PowerByTier.Count - 1;
            List<int> list = level2PowerByTier[index];
            int num = list[list.Count - 1];
            return num * slotCount;
        }

        int CollectPercentSum(List<int[]> modulesParams, List<List<int>> level2PowerByTier) {
            int num = 0;

            foreach (int[] modulesParam in modulesParams) {
                int index = modulesParam[0];
                int index2 = Mathf.Min(modulesParam[1], level2PowerByTier[index].Count - 1);
                num += level2PowerByTier[index][index2];
            }

            return num;
        }
    }
}