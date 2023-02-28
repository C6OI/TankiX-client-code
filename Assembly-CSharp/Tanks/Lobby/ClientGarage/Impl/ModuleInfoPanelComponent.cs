using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleInfoPanelComponent : LocalizedControl, Component {
        [SerializeField] Text slotText;

        [SerializeField] Text moduleNameText;

        [SerializeField] Text mountLabelText;

        [SerializeField] RectTransform slotInfoPanel;

        [SerializeField] ImageSkin slotInfoSlotIcon;

        [SerializeField] ImageSkin slotInfoModuleIcon;

        [SerializeField] ImageSkin slotInfoLockIcon;

        [SerializeField] Text moduleDescriptionText;

        [SerializeField] CardPriceLabelComponent priceLabel;

        [SerializeField] Text moduleExceptionalText;

        [SerializeField] Text moduleEpicText;

        [SerializeField] GameObject defenceIcon;

        [SerializeField] GameObject scoutingIcon;

        [SerializeField] GameObject specialIcon;

        [SerializeField] GameObject supportIcon;

        [Header("Localization")] [SerializeField]
        Text specialText;

        [SerializeField] Text scoutingText;

        [SerializeField] Text defenceText;

        [SerializeField] Text supportText;

        public string SlotText {
            set => slotText.text = value;
        }

        public string ModuleNameText {
            set => moduleNameText.text = value;
        }

        public string MountLabelText {
            set => mountLabelText.text = value;
        }

        public RectTransform SlotInfoPanel => slotInfoPanel;

        public ImageSkin SlotInfoSlotIcon => slotInfoSlotIcon;

        public ImageSkin SlotInfoModuleIcon => slotInfoModuleIcon;

        public ImageSkin SlotInfoLockIcon => slotInfoLockIcon;

        public string ModuleDescriptionText {
            set => moduleDescriptionText.text = value;
        }

        public CardPriceLabelComponent PriceLabel => priceLabel;

        public Text ModuleExceptionalText => moduleExceptionalText;

        public Text ModuleEpicText => moduleEpicText;

        public GameObject DefenceIcon => defenceIcon;

        public GameObject ScoutingIcon => scoutingIcon;

        public GameObject SpecialIcon => specialIcon;

        public GameObject SupportIcon => supportIcon;

        public string SpecialText {
            get => specialText.text;
            set => specialText.text = value;
        }

        public string ScoutingText {
            get => scoutingText.text;
            set => scoutingText.text = value;
        }

        public string DefenceText {
            get => defenceText.text;
            set => defenceText.text = value;
        }

        public string SupportText {
            get => supportText.text;
            set => supportText.text = value;
        }

        public void ScrollUpDescription() {
            ((RectTransform)moduleDescriptionText.transform).anchoredPosition = default;
        }
    }
}