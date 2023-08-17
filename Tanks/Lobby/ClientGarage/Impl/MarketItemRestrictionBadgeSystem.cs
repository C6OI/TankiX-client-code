using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MarketItemRestrictionBadgeSystem : ECSSystem {
        [OnEventFire]
        public void ShowUserRankRestrictionIndicator(NodeAddedEvent e, [Combine] MarketItemWithUserRankRestrictionNode item,
            SelfUserNode selfUser) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, item);

            if (checkMarketItemRestrictionsEvent.RestrictedByRank) {
                item.userRankRestrictionBadgeGUI.SetRank(item.purchaseUserRankRestriction.RestrictionValue + 1);
                item.userRankRestrictionBadgeGUI.gameObject.SetActive(true);
                item.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
            }
        }

        [OnEventFire]
        public void ShowUserRankRestrictionIndicator(UpdateRankEvent e, SelfUserNode selfUser,
            [Combine] [JoinAll] MarketItemWithUserRankRestrictionNode item) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, item);

            if (!checkMarketItemRestrictionsEvent.RestrictedByRank) {
                item.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
                item.userRankRestrictionBadgeGUI.gameObject.SetActive(false);
            }
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e,
            MarketItemWithUpgradeLevelRestrictionNode marketItem) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, marketItem);

            if (checkMarketItemRestrictionsEvent.RestrictedByUpgradeLevel) {
                marketItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue =
                    marketItem.purchaseUpgradeLevelRestriction.RestrictionValue.ToString();

                marketItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);

                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled",
                    SendMessageOptions.RequireReceiver);
            }
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(ItemUpgradedEvent e, UpgradableItemNode parentItem,
            [Combine] [JoinByParentGroup] MarketItemWithUpgradeLevelRestrictionNode marketItem) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, marketItem);

            if (!checkMarketItemRestrictionsEvent.RestrictedByUpgradeLevel &&
                !checkMarketItemRestrictionsEvent.RestrictedByRank &&
                marketItem.upgradeLevelRestrictionBadgeGUI.gameObject.activeSelf) {
                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled",
                    SendMessageOptions.DontRequireReceiver);

                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("Unlock",
                    SendMessageOptions.DontRequireReceiver);

                marketItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(false);

                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("MoveToItem",
                    marketItem.upgradeLevelRestrictionBadgeGUI.gameObject,
                    SendMessageOptions.DontRequireReceiver);
            }
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class MarketItemWithUserRankRestrictionNode : MarketItemNode {
            public PurchaseUserRankRestrictionComponent purchaseUserRankRestriction;

            public UserRankRestrictionBadgeGUIComponent userRankRestrictionBadgeGUI;
        }

        public class MarketItemWithUpgradeLevelRestrictionNode : MarketItemNode {
            public ParentGroupComponent parentGroup;
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;

            public UpgradeLevelRestrictionBadgeGUIComponent upgradeLevelRestrictionBadgeGUI;
        }

        public class UpgradableItemNode : Node {
            public ParentGroupComponent parentGroup;
            public UpgradeLevelItemComponent upgradeLevelItem;

            public UserItemComponent userItem;
        }
    }
}