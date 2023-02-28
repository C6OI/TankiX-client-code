using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesScreenUIComponent : BehaviourComponent {
        [SerializeField] SlotUIComponent[] slots;

        [SerializeField] CanvasGroup turretSlots;

        [SerializeField] CanvasGroup hullSlots;

        [SerializeField] ModuleCardItemUIComponent moduleCardItemUiComponentPrefab;

        [SerializeField] ModuleCardsTierUI[] tiersUi;

        [SerializeField] TextMeshProUGUI moduleName;

        [SerializeField] TextMeshProUGUI moduleDesc;

        [SerializeField] TextMeshProUGUI moduleFlavorText;

        [SerializeField] ImageSkin moduleIcon;

        [SerializeField] TextMeshProUGUI tankPartItemName;

        [SerializeField] TextMeshProUGUI moduleTypeName;

        [SerializeField] TextMeshProUGUI currentUpgradeLevel;

        [SerializeField] TextMeshProUGUI nextUpgradeLevel;

        [SerializeField] TextMeshProUGUI upgradeTitle;

        [SerializeField] LocalizedField activeType;

        [SerializeField] LocalizedField passiveType;

        [SerializeField] LocalizedField upgradeLevel;

        [SerializeField] LocalizedField hullHealth;

        [SerializeField] LocalizedField turretDamage;

        [SerializeField] ModulesPropertiesUIComponent modulesProperties;

        [SerializeField] TankPartItemPropertiesUIComponent tankPartItemPropertiesUI;

        [SerializeField] TutorialShowTriggerComponent upgradeModuleTrigger;

        public Action onEanble;

        public string HullHealth => hullHealth.Value;

        public string TurretDamage => turretDamage.Value;

        public TankPartItem CurrentTankPartItem { get; private set; }

        public ModulesPropertiesUIComponent ModulesProperties => modulesProperties;

        public string ModuleName {
            set => moduleName.text = value;
        }

        public string ModuleDesc {
            set => moduleDesc.text = value;
        }

        public string ModuleFlavorText {
            set => moduleFlavorText.text = value;
        }

        public bool ModuleActive {
            set => moduleTypeName.text = !value ? passiveType.Value : activeType.Value;
        }

        public string ModuleIconUID {
            set => moduleIcon.SpriteUid = value;
        }

        public int CurrentUpgradeLevel {
            set {
                currentUpgradeLevel.text = upgradeLevel.Value + " " + value;
                currentUpgradeLevel.gameObject.SetActive(value >= 0);
                upgradeTitle.text = value >= 0 ? upgradeLevel.Value + " " + value : string.Empty;
            }
        }

        public int NextUpgradeLevel {
            set {
                nextUpgradeLevel.text = upgradeLevel.Value + " " + value;
                nextUpgradeLevel.gameObject.SetActive(value >= 0);
            }
        }

        void Reset() {
            tiersUi = GetComponentsInChildren<ModuleCardsTierUI>();
        }

        void OnEnable() {
            if (onEanble != null) {
                onEanble();
            }
        }

        public SlotUIComponent GetSlot(Slot slot) => slots[(uint)slot];

        public void SetItem(TankPartItem item) {
            CurrentTankPartItem = item;
            tankPartItemName.text = item.Name;
            FilterSlots();
        }

        public void FilterSlots() {
            SwitchSlots(turretSlots, CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Turret);
            SwitchSlots(hullSlots, CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Hull);
            int num = CurrentTankPartItem.Type != 0 ? 3 : 0;
            ToggleListItemComponent component = slots[num].GetComponent<ToggleListItemComponent>();
            component.Toggle.isOn = true;
        }

        void SwitchSlots(CanvasGroup slots, bool isOn) {
            slots.interactable = isOn;
            slots.alpha = !isOn ? 0f : 1f;
            slots.blocksRaycasts = isOn;
        }

        public void AddCard(Entity entity) {
            int tierNumber = entity.GetComponent<ModuleTierComponent>().TierNumber;
            tierNumber = Mathf.Min(tierNumber, tiersUi.Length - 1);
            ModuleCardItemUIComponent moduleCardItemUIComponent = Instantiate(moduleCardItemUiComponentPrefab);
            tiersUi[tierNumber].AddCard(moduleCardItemUIComponent);

            if (entity.HasComponent<ModuleCardItemUIComponent>()) {
                entity.RemoveComponent<ModuleCardItemUIComponent>();
            }

            if (entity.HasComponent<ToggleListItemComponent>()) {
                entity.RemoveComponent<ToggleListItemComponent>();
            }

            entity.AddComponent(moduleCardItemUIComponent);
            entity.AddComponent(moduleCardItemUIComponent.GetComponent<ToggleListItemComponent>());
        }

        public ModuleCardItemUIComponent GetCard(long marketItemGroupId) {
            ModuleCardsTierUI[] array = tiersUi;

            foreach (ModuleCardsTierUI moduleCardsTierUI in array) {
                ModuleCardItemUIComponent card = moduleCardsTierUI.GetCard(marketItemGroupId);

                if (card != null) {
                    return card;
                }
            }

            return null;
        }

        public void FilterCards(Entity mountedModule, ModuleBehaviourType slotType) {
            if (CurrentTankPartItem == null) {
                return;
            }

            ModuleCardItemUIComponent[] componentsInChildren = GetComponentsInChildren<ModuleCardItemUIComponent>(true);
            bool flag = false;
            ModuleCardItemUIComponent[] array = componentsInChildren;

            foreach (ModuleCardItemUIComponent moduleCardItemUIComponent in array) {
                if (moduleCardItemUIComponent.gameObject.activeSelf && moduleCardItemUIComponent.GetComponent<ToggleListItemComponent>().Toggle.isOn) {
                    flag = moduleCardItemUIComponent.Type == slotType &&
                           (moduleCardItemUIComponent.TankPart == TankPartModuleType.WEAPON && CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Turret ||
                            moduleCardItemUIComponent.TankPart == TankPartModuleType.TANK && CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Hull);

                    break;
                }
            }

            ModuleCardItemUIComponent[] array2 = componentsInChildren;

            foreach (ModuleCardItemUIComponent moduleCardItemUIComponent2 in array2) {
                bool flag2 = moduleCardItemUIComponent2.TankPart == TankPartModuleType.WEAPON && CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Turret ||
                             moduleCardItemUIComponent2.TankPart == TankPartModuleType.TANK && CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Hull;

                moduleCardItemUIComponent2.gameObject.SetActive(flag2);

                if (flag2 && (mountedModule != null && moduleCardItemUIComponent2.ModuleEntity == mountedModule ||
                              mountedModule == null && !flag && moduleCardItemUIComponent2.Type == slotType)) {
                    moduleCardItemUIComponent2.GetComponent<ToggleListItemComponent>().Toggle.isOn = true;
                    flag = true;
                }
            }

            upgradeModuleTrigger.Triggered();
        }

        public void Clear() {
            ModuleCardsTierUI[] array = tiersUi;

            foreach (ModuleCardsTierUI moduleCardsTierUI in array) {
                moduleCardsTierUI.Clear();
            }
        }

        public void ShowTankItemStat(float tankCoef, float weaponCoef, float tankCoefWithSelected, float weaponCoefWithSelected) {
            if (CurrentTankPartItem.Type == TankPartItem.TankPartItemType.Hull) {
                tankPartItemPropertiesUI.Show(CurrentTankPartItem, tankCoef, tankCoefWithSelected);
            } else {
                tankPartItemPropertiesUI.Show(CurrentTankPartItem, weaponCoef, weaponCoefWithSelected);
            }
        }
    }
}