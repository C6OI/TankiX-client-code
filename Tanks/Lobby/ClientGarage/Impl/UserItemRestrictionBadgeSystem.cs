using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UserItemRestrictionBadgeSystem : ECSSystem {
        [OnEventFire]
        public void ShowUserRankRestrictionIndicator(NodeAddedEvent e, UserItemWithUserRankRestrictionNode userItem,
            [JoinByMarketItem] [Context] MarketItemWithUserRankRestrictionNode marketItem) {
            userItem.userRankRestrictionBadgeGUI.SetRank(marketItem.mountUserRankRestriction.RestrictionValue + 1);
            userItem.userRankRestrictionBadgeGUI.gameObject.SetActive(true);
            userItem.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void HideUserRankRestrictionIndicator(NodeRemoveEvent e, UserItemWithUserRankRestrictionNode userItem) {
            userItem.userRankRestrictionBadgeGUI.gameObject.SetActive(false);
            userItem.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e,
            NotGraffitiUserItemWithUpgradeLevelRestrictionNode userItem,
            [JoinByMarketItem] [Context] MarketItemWithUpgradeLevelRestrictionNode marketItem) {
            userItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue =
                marketItem.mountUpgradeLevelRestriction.RestrictionValue.ToString();

            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);

            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled",
                SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e,
            GraffitiUserItemWithUpgradeLevelRestrictionNode userItem,
            [Context] [JoinByMarketItem] MarketItemWithUpgradeLevelRestrictionNode marketItem,
            [JoinByParentGroup] ParentWeaponMarketItemNode parent) {
            userItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue = string.Format("{0} {1}",
                parent.descriptionItem.name,
                marketItem.mountUpgradeLevelRestriction.RestrictionValue);

            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);

            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled",
                SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e,
            GraffitiUserItemWithUpgradeLevelRestrictionNode userItem,
            [JoinByMarketItem] [Context] MarketItemWithUpgradeLevelRestrictionNode marketItem,
            [JoinByParentGroup] ParentHullMarketItemNode parent) {
            userItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue = string.Format("{0} {1}",
                parent.descriptionItem.name,
                marketItem.mountUpgradeLevelRestriction.RestrictionValue);

            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);

            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled",
                SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void HideUpgradeLevelRestrictionIndicator(NodeRemoveEvent e,
            UserItemWithUpgradeLevelRestrictionNode userItem) {
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("Unlock", SendMessageOptions.DontRequireReceiver);

            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled",
                SendMessageOptions.DontRequireReceiver);

            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(false);

            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("MoveToItem",
                userItem.upgradeLevelRestrictionBadgeGUI.gameObject,
                SendMessageOptions.DontRequireReceiver);
        }

        public class UserItemNode : Node {
            public UserItemComponent userItem;
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;
        }

        public class MarketItemWithUserRankRestrictionNode : MarketItemNode {
            public MountUserRankRestrictionComponent mountUserRankRestriction;
        }

        public class MarketItemWithUpgradeLevelRestrictionNode : MarketItemNode {
            public MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction;
        }

        public class UserItemWithUserRankRestrictionNode : UserItemNode {
            public RestrictedByUserRankComponent restrictedByUserRank;

            public UserRankRestrictionBadgeGUIComponent userRankRestrictionBadgeGUI;
        }

        public class UserItemWithUpgradeLevelRestrictionNode : UserItemNode {
            public RestrictedByUpgradeLevelComponent restrictedByUpgradeLevel;

            public UpgradeLevelRestrictionBadgeGUIComponent upgradeLevelRestrictionBadgeGUI;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class NotGraffitiUserItemWithUpgradeLevelRestrictionNode : UserItemWithUpgradeLevelRestrictionNode { }

        public class GraffitiUserItemWithUpgradeLevelRestrictionNode : UserItemWithUpgradeLevelRestrictionNode {
            public GraffitiItemComponent graffitiItem;
        }

        public class ParentHullMarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public MarketItemComponent marketItem;

            public ParentGroupComponent parentGroup;
            public TankItemComponent tankItem;
        }

        public class ParentWeaponMarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public MarketItemComponent marketItem;

            public ParentGroupComponent parentGroup;
            public WeaponItemComponent weaponItem;
        }
    }
}