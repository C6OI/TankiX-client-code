using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemsScreenComponent : MonoBehaviour, Component {
        [SerializeField] GameObject buyButton;

        [SerializeField] GameObject xBuyButton;

        [SerializeField] MountLabelComponent mountLabel;

        [SerializeField] MountItemButtonComponent mountItemButton;

        [SerializeField] ItemPropertiesButtonComponent itemPropertiesButton;

        [SerializeField] UserRankRestrictionDescriptionGUIComponent userRankRestrictionDescription;

        [SerializeField] UpgradeLevelRestrictionDescriptionGUIComponent upgradeLevelRestrictionDescription;

        [SerializeField] Text onlyInContainerLabel;

        [SerializeField] GoToContainersScreenButtonComponent containersButton;

        public GameObject BuyButton => buyButton;

        public GameObject XBuyButton => xBuyButton;

        public MountLabelComponent MountLabel => mountLabel;

        public MountItemButtonComponent MountItemButton => mountItemButton;

        public ItemPropertiesButtonComponent ItemPropertiesButton => itemPropertiesButton;

        public UserRankRestrictionDescriptionGUIComponent UserRankRestrictionDescription => userRankRestrictionDescription;

        public UpgradeLevelRestrictionDescriptionGUIComponent UpgradeLevelRestrictionDescription => upgradeLevelRestrictionDescription;

        public bool OnlyInContainerUIVisibility {
            set {
                onlyInContainerLabel.gameObject.SetActive(value);
                containersButton.gameObject.SetActive(value);
            }
        }

        public bool OnlyInContainerLabelVisibility {
            set => onlyInContainerLabel.gameObject.SetActive(value);
        }

        public bool InContainerButtonVisibility {
            set => containersButton.gameObject.SetActive(value);
        }

        public string OnlyInContainerLabel {
            set => onlyInContainerLabel.text = value;
        }
    }
}