using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayPropertyButtonSystem : ECSSystem {
        [OnEventFire]
        public void ShowUpgradeTextForUpgradableItem(ListItemSelectedEvent e, UpgradableItem item,
            [JoinAll] ScreenNode screen) {
            UpgradeButtonComponent upgradeItemButton = screen.garageItemsScreen.UpgradeItemButton;
            ItemPropertiesButtonComponent itemPropertiesButton = screen.garageItemsScreen.ItemPropertiesButton;
            upgradeItemButton.gameObject.SetActive(true);
            itemPropertiesButton.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(upgradeItemButton.gameObject);
        }

        [OnEventFire]
        public void ShowPropertiesTextForMarketItem(ListItemSelectedEvent e, MarketItemWithProperties item,
            [JoinAll] ScreenNode screen) {
            UpgradeButtonComponent upgradeItemButton = screen.garageItemsScreen.UpgradeItemButton;
            ItemPropertiesButtonComponent itemPropertiesButton = screen.garageItemsScreen.ItemPropertiesButton;
            upgradeItemButton.gameObject.SetActive(false);
            itemPropertiesButton.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void HidePropertiesButtonForGraffiti(ListItemSelectedEvent e, SingleNode<GraffitiItemComponent> graffiti,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<ItemPropertiesButtonComponent> propertyButton) =>
            propertyButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HideUpgradeButtonForGraffiti(ListItemSelectedEvent e, SingleNode<GraffitiItemComponent> graffiti,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<UpgradeButtonComponent> upgradeButton) =>
            upgradeButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HidePropertiesButtonForPaint(ListItemSelectedEvent e, SingleNode<PaintItemComponent> paint,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<ItemPropertiesButtonComponent> propertyButton) =>
            propertyButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HideUpgradeButtonForPaint(ListItemSelectedEvent e, SingleNode<PaintItemComponent> paint,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<UpgradeButtonComponent> upgradeButton) =>
            upgradeButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HideUpgradeButtonForShell(ListItemSelectedEvent e, SingleNode<ShellItemComponent> shell,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<UpgradeButtonComponent> upgradeButton) =>
            upgradeButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HidePropertiesButtonForShell(ListItemSelectedEvent e, SingleNode<ShellItemComponent> shell,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<ItemPropertiesButtonComponent> propertyButton) =>
            propertyButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HideUpgradeButtonForShell(ListItemSelectedEvent e, SingleNode<SkinItemComponent> shell,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<UpgradeButtonComponent> upgradeButton) =>
            upgradeButton.component.gameObject.SetActive(false);

        [OnEventComplete]
        public void HidePropertiesButtonForShell(ListItemSelectedEvent e, SingleNode<SkinItemComponent> shell,
            [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<ItemPropertiesButtonComponent> propertyButton) =>
            propertyButton.component.gameObject.SetActive(false);

        [OnEventFire]
        public void AddPropertyButton(NodeAddedEvent e, PropertyButtonNode propertyButton) {
            Text text = propertyButton.itemPropertiesButton.gameObject.GetComponentsInChildren<Text>(true)[0];
            text.text = propertyButton.propertyItemButtonText.PropertyText;
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GarageItemsScreenComponent garageItemsScreen;
            public ItemsListScreenComponent itemsListScreen;

            public ScreenComponent screen;

            public ScreenGroupComponent screenGroup;
        }

        public class UpgradableItem : Node {
            public UpgradableItemComponent upgradableItem;

            public UserItemComponent userItem;
        }

        [Not(typeof(PaintItemComponent))]
        [Not(typeof(GraffitiItemComponent))]
        public class MarketItemWithProperties : Node {
            public MarketItemComponent marketItem;
        }

        public class PropertyButtonNode : Node {
            public ItemPropertiesButtonComponent itemPropertiesButton;
            public PropertyItemButtonTextComponent propertyItemButtonText;
        }
    }
}