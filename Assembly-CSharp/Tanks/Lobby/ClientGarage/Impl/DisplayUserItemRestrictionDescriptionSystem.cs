using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayUserItemRestrictionDescriptionSystem : ECSSystem {
        public static readonly string RANK = "%RANK%";

        public static readonly string ITEM_upgLEVEL = "%ITEM_upgLEVEL%";

        public static readonly string ITEM_NAME = "%ITEM_NAME%";

        [OnEventFire]
        public void HideDescriptions(ListItemSelectedEvent e, Node any, [JoinAll] ScreenNode screen) {
            screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(false);
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(false);
        }

        [OnEventComplete]
        public void ShowUserRankRestrictionDescription(ListItemSelectedEvent e, UserRankRestrictionNode userRankRestriction,
            [JoinByMarketItem] SingleNode<MountUserRankRestrictionComponent> marketItem, [JoinAll] ScreenNode screen, [JoinAll] SingleNode<RanksNamesComponent> ranksNames) {
            ShowUserRankRestrictionDescription(screen, marketItem.component, ranksNames.component);
        }

        void ShowUserRankRestrictionDescription(ScreenNode screen, MountUserRankRestrictionComponent mountUserRankRestriction, RanksNamesComponent ranksNames) {
            screen.garageItemsScreen.UserRankRestrictionDescription.Description =
                screen.garageItemsScreenText.UserRankRestrictionDescription.Replace(RANK, ranksNames.Names[mountUserRankRestriction.RestrictionValue]);

            screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void HideUserRankRestrictionDescription(NodeRemoveEvent e, UserRankRestrictionNode itemWithUserRankRestriction, [JoinAll] ScreenNode screen,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) {
            if (selectedItem.component.SelectedItem == itemWithUserRankRestriction.Entity) {
                screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(false);
            }
        }

        [OnEventComplete]
        public void ShowUpgradeLevelRestrictionDescription(ListItemSelectedEvent e, UpgradeLevelRestrictionNotGraffitiNode upgradeLevelRestriction,
            [JoinByMarketItem] SingleNode<MountUpgradeLevelRestrictionComponent> marketItem, [JoinAll] ScreenNode screen) {
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.Description =
                screen.garageItemsScreenText.UpgradeLevelRestrictionDescription.Replace(ITEM_upgLEVEL, marketItem.component.RestrictionValue.ToString());

            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void ShowUpgradeLevelRestrictionDescription(ListItemSelectedEvent e, UpgradeLevelRestrictionGraffitiNode upgradeLevelRestriction,
            [JoinByMarketItem] SingleNode<MountUpgradeLevelRestrictionComponent> marketItem, [JoinByParentGroup] WeaponNode weapon, [JoinAll] ScreenNode screen) {
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.Description = screen.garageItemsScreenText.WeaponUpgradeLevelRestrictionDescription
                .Replace(ITEM_upgLEVEL, marketItem.component.RestrictionValue.ToString()).Replace(ITEM_NAME, weapon.descriptionItem.Name);

            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void ShowUpgradeLevelRestrictionDescription(ListItemSelectedEvent e, UpgradeLevelRestrictionGraffitiNode upgradeLevelRestriction,
            [JoinByMarketItem] SingleNode<MountUpgradeLevelRestrictionComponent> marketItem, [JoinByParentGroup] HullNode hull, [JoinAll] ScreenNode screen) {
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.Description = screen.garageItemsScreenText.HullUpgradeLevelRestrictionDescription
                .Replace(ITEM_upgLEVEL, marketItem.component.RestrictionValue.ToString()).Replace(ITEM_NAME, hull.descriptionItem.Name);

            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void HideUpgradeLevelRestrictionDescription(NodeRemoveEvent e, UpgradeLevelRestrictionNode itemWithUpgradeLevelRestriction, [JoinAll] ScreenNode screen,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) {
            if (selectedItem.component.SelectedItem == itemWithUpgradeLevelRestriction.Entity) {
                screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(false);
            }
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenComponent garageItemsScreen;

            public GarageItemsScreenTextComponent garageItemsScreenText;

            public ScreenComponent screen;
        }

        public class UserItemNode : Node {
            public UserItemComponent userItem;
        }

        public class UserRankRestrictionNode : UserItemNode {
            public RestrictedByUserRankComponent restrictedByUserRank;
        }

        public class UpgradeLevelRestrictionNode : UserItemNode {
            public RestrictedByUpgradeLevelComponent restrictedByUpgradeLevel;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class UpgradeLevelRestrictionNotGraffitiNode : UpgradeLevelRestrictionNode { }

        public class UpgradeLevelRestrictionGraffitiNode : UpgradeLevelRestrictionNode {
            public GraffitiItemComponent graffitiItem;
        }

        public class WeaponNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;

            public WeaponItemComponent weaponItem;
        }

        public class HullNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;

            public TankItemComponent tankItem;
        }
    }
}