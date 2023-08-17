using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayMarketItemRestrictionDescriptionSystem : ECSSystem {
        public static readonly string RANK = "%RANK%";

        public static readonly string ITEM_upgLEVEL = "%ITEM_upgLEVEL%";

        [OnEventFire]
        public void HideDescriptions(ListItemSelectedEvent e, Node any, [JoinAll] ScreenNode screen) {
            screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(false);
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(false);
        }

        [OnEventComplete]
        public void ShowUserRankRestrictionDescription(ListItemSelectedEvent e, UserRankRestrictionNode userRankRestriction,
            [JoinAll] SelfUserNode user, [JoinAll] ScreenNode screen, [JoinAll] SingleNode<RanksNamesComponent> ranksNames) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, userRankRestriction);

            if (checkMarketItemRestrictionsEvent.RestrictedByRank) {
                ShowUserRankRestrictionDescription(screen,
                    userRankRestriction.purchaseUserRankRestriction,
                    ranksNames.component);
            }
        }

        void ShowUserRankRestrictionDescription(ScreenNode screen, PurchaseUserRankRestrictionComponent restriction,
            RanksNamesComponent ranksNames) {
            screen.garageItemsScreen.UserRankRestrictionDescription.Description =
                screen.garageItemsScreenText.UserRankRestrictionDescription.Replace(RANK,
                    ranksNames.Names[restriction.RestrictionValue]);

            screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void ShowUpgradeLevelRestrictionDescription(ListItemSelectedEvent e,
            UpgradeLevelRestrictionNode upgradeLevelRestriction, [JoinAll] ScreenNode screen) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, upgradeLevelRestriction);

            if (checkMarketItemRestrictionsEvent.RestrictedByUpgradeLevel) {
                ShowUpgradeLevelRestrictionDescription(screen, upgradeLevelRestriction.purchaseUpgradeLevelRestriction);
            }
        }

        void ShowUpgradeLevelRestrictionDescription(ScreenNode screen,
            PurchaseUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction) {
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.Description =
                screen.garageItemsScreenText.UpgradeLevelRestrictionDescription.Replace(ITEM_upgLEVEL,
                    mountUpgradeLevelRestriction.RestrictionValue.ToString());

            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(true);
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenComponent garageItemsScreen;

            public GarageItemsScreenTextComponent garageItemsScreenText;

            public ScreenComponent screen;
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;
        }

        public class UpgradableItemNode : Node {
            public ParentGroupComponent parentGroup;
            public UpgradeLevelItemComponent upgradeLevelItem;

            public UserItemComponent userItem;
        }

        public class UserRankRestrictionNode : MarketItemNode {
            public PurchaseUserRankRestrictionComponent purchaseUserRankRestriction;
        }

        public class UpgradeLevelRestrictionNode : MarketItemNode {
            public ParentGroupComponent parentGroup;
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }
    }
}