using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleCardItemUIComponent : BehaviourComponent, AttachToEntityListener {
        public ModuleBehaviourType Type;

        public TankPartModuleType TankPart;

        [SerializeField] ImageSkin icon;

        [SerializeField] TextMeshProUGUI levelLabel;

        [SerializeField] TextMeshProUGUI moduleName;

        [SerializeField] GameObject selectBorder;

        [SerializeField] Slider upgradeSlider;

        [SerializeField] TextMeshProUGUI upgradeLabel;

        [SerializeField] GameObject activeBorder;

        [SerializeField] GameObject passiveBorder;

        [SerializeField] LocalizedField activateAvailableLocalizedField;

        [SerializeField] LocalizedField upgradeAvailableLocalizedField;

        [SerializeField] TextMeshProUGUI upgradeAvailableText;

        [SerializeField] float notMountableAlpha = 0.3f;

        public bool mountable;

        int cardsCount;

        int level = -1;

        int maxCardsCount;

        bool upgradeAvailable;

        public Entity ModuleEntity { get; private set; }

        public long MarketGroupId {
            get {
                if (ModuleEntity != null && ModuleEntity.HasComponent<MarketItemGroupComponent>()) {
                    return ModuleEntity.GetComponent<MarketItemGroupComponent>().Key;
                }

                return 0L;
            }
        }

        public string ModuleName {
            set => moduleName.text = value;
        }

        public bool Active {
            set {
                activeBorder.SetActive(value);
                passiveBorder.SetActive(!value);
            }
        }

        public bool UpgradeAvailable {
            get => upgradeAvailable;
            set {
                upgradeAvailable = value;
                SetUpgradeAvailableText(value, upgradeAvailableLocalizedField.Value);
            }
        }

        public bool ActivateAvailable {
            get => upgradeAvailable;
            set {
                upgradeAvailable = value;
                SetUpgradeAvailableText(value, activateAvailableLocalizedField.Value);
            }
        }

        public int Level {
            get => level;
            set {
                level = value;

                if (level == -1) {
                    levelLabel.text = "0";
                } else {
                    levelLabel.text = level.ToString();
                }

                Mountable = level > 0 || UpgradeAvailable;
            }
        }

        public string Icon {
            set => icon.SpriteUid = value;
        }

        public bool Selected {
            set => selectBorder.SetActive(value);
        }

        public int MaxCardsCount {
            get => maxCardsCount;
            set {
                maxCardsCount = value;
                upgradeSlider.maxValue = maxCardsCount;
                CardsCount = cardsCount;
                upgradeSlider.transform.parent.gameObject.SetActive(maxCardsCount > 0);
            }
        }

        public int CardsCount {
            get => cardsCount;
            set {
                cardsCount = value;
                upgradeLabel.text = MaxCardsCount <= 0 ? "MAX" : cardsCount + "/" + MaxCardsCount;
                upgradeSlider.value = cardsCount;

                if (level > 0) {
                    UpgradeAvailable = cardsCount >= maxCardsCount && maxCardsCount > 0;
                } else {
                    ActivateAvailable = cardsCount >= maxCardsCount && maxCardsCount > 0;
                }
            }
        }

        public bool Mountable {
            get => mountable;
            set {
                mountable = value;
                GetComponent<CanvasGroup>().alpha = !value ? notMountableAlpha : 1f;
                CanvasGroup component = selectBorder.GetComponent<CanvasGroup>();
                bool ignoreParentGroups = !value;
                upgradeAvailableText.GetComponent<CanvasGroup>().ignoreParentGroups = ignoreParentGroups;
                component.ignoreParentGroups = ignoreParentGroups;
            }
        }

        void OnDestroy() {
            if (ModuleEntity != null && ClientUnityIntegrationUtils.HasEngine()) {
                ModuleEntity.RemoveComponent<ModuleCardItemUIComponent>();
            }
        }

        public void AttachedToEntity(Entity entity) {
            ModuleEntity = entity;
        }

        public void SetCardsCount(int value, bool activate) { }

        void SetUpgradeAvailableText(bool available, string text) {
            upgradeAvailableText.gameObject.SetActive(available);
            upgradeAvailableText.text = text;

            if (available && !Mountable) {
                Mountable = true;
            }
        }

        public void Select() {
            Selected = true;
        }
    }
}