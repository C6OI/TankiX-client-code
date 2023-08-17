using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MarketItemRestrictionCheckSystem : ECSSystem {
        [OnEventFire]
        public void RestrictMarketItemByUpgradeLevel(CheckMarketItemRestrictionsEvent e,
            MarketItemWithUpgradeLevelRestrictionNode marketItem, [JoinAll] SelfUserNode selfUser,
            [JoinByUser] ICollection<UpgradableItemNode> items) {
            Entity entityById = GetEntityById(marketItem.parentGroup.Key);

            foreach (UpgradableItemNode item in items) {
                if (item.marketItemGroup.Key == entityById.Id) {
                    long level = item.upgradeLevelItem.Level;
                    e.RestrictByUpgradeLevel(level < marketItem.purchaseUpgradeLevelRestriction.RestrictionValue);
                }
            }
        }

        [OnEventFire]
        public void RestrictMarketItemByUserRank(CheckMarketItemRestrictionsEvent e,
            [Combine] MarketItemWithUserRankRestrictionNode item, [JoinAll] SelfUserNode selfUser) =>
            e.RestrictByRank(item.purchaseUserRankRestriction.RestrictionValue > selfUser.userRank.Rank);

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class MarketItemWithUpgradeLevelRestrictionNode : MarketItemNode {
            public ParentGroupComponent parentGroup;
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;
        }

        public class MarketItemWithUserRankRestrictionNode : MarketItemNode {
            public PurchaseUserRankRestrictionComponent purchaseUserRankRestriction;
        }

        public class UpgradableItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public ParentGroupComponent parentGroup;
            public UpgradeLevelItemComponent upgradeLevelItem;

            public UserItemComponent userItem;
        }
    }
}