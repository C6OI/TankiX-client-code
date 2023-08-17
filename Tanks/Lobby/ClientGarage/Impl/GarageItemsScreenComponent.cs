using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemsScreenComponent : MonoBehaviour, Component {
        [SerializeField] GameObject buyButton;

        [SerializeField] GameObject xBuyButton;

        [SerializeField] MountLabelComponent mountLabel;

        [SerializeField] MountItemButtonComponent mountItemButton;

        [SerializeField] ItemPropertiesButtonComponent itemPropertiesButton;

        [SerializeField] UpgradeButtonComponent upgradeItemButton;

        [SerializeField] UserRankRestrictionDescriptionGUIComponent userRankRestrictionDescription;

        [SerializeField] UpgradeLevelRestrictionDescriptionGUIComponent upgradeLevelRestrictionDescription;

        public GameObject BuyButton => buyButton;

        public GameObject XBuyButton => xBuyButton;

        public MountLabelComponent MountLabel => mountLabel;

        public MountItemButtonComponent MountItemButton => mountItemButton;

        public ItemPropertiesButtonComponent ItemPropertiesButton => itemPropertiesButton;

        public UpgradeButtonComponent UpgradeItemButton => upgradeItemButton;

        public UserRankRestrictionDescriptionGUIComponent UserRankRestrictionDescription => userRankRestrictionDescription;

        public UpgradeLevelRestrictionDescriptionGUIComponent UpgradeLevelRestrictionDescription =>
            upgradeLevelRestrictionDescription;
    }
}