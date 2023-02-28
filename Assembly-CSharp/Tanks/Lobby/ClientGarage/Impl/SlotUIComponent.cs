using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotUIComponent : UIBehaviour, Component, AttachToEntityListener, IPointerClickHandler, IEventSystemHandler {
        [SerializeField] ImageSkin moduleIconImageSkin;

        [SerializeField] PaletteColorField exceptionalColor;

        [SerializeField] PaletteColorField epicColor;

        [SerializeField] Image moduleIcon;

        [SerializeField] Image selectionImage;

        [SerializeField] Image lockIcon;

        [SerializeField] Image upgradeIcon;

        [SerializeField] TextMeshProUGUI slotName;

        [SerializeField] LocalizedField slotNameLocalization;

        bool locked;

        public Slot Slot {
            set => slotName.text = slotNameLocalization.Value + " " + (int)(value + 1);
        }

        public TankPartModuleType TankPart { get; set; }

        public ImageSkin ModuleIconImageSkin => moduleIconImageSkin;

        public Image ModuleIcon {
            get => moduleIcon;
            set => moduleIcon = value;
        }

        public Image SelectionImage {
            get => selectionImage;
            set => selectionImage = value;
        }

        public Image UpgradeIcon {
            get => upgradeIcon;
            set => upgradeIcon = value;
        }

        public Color ExceptionalColor => exceptionalColor;

        public Color EpicColor => epicColor;

        public bool Locked {
            get => locked;
            set {
                lockIcon.gameObject.SetActive(value);
                locked = value;
                GetComponent<CanvasGroup>().alpha = !locked ? 1f : 0.6f;
                Toggle component = GetComponent<Toggle>();

                if (component != null) {
                    component.interactable = !locked;
                }
            }
        }

        public int Rank { get; set; }

        public Entity SlotEntity { get; private set; }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (SlotEntity != null && ClientUnityIntegrationUtils.HasEngine() && SlotEntity.HasComponent<ModuleCardItemUIComponent>()) {
                SlotEntity.RemoveComponent<ModuleCardItemUIComponent>();
            }
        }

        public void AttachedToEntity(Entity entity) {
            SlotEntity = entity;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (locked) {
                GetComponent<SlotTooltipShowComponent>().ShowTooltip(Input.mousePosition);
            }
        }
    }
}