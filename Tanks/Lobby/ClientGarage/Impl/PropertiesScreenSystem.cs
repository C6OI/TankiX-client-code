using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PropertiesScreenSystem : ECSSystem {
        public static readonly string ITEM_NAME = "%ITEM_NAME%";

        [OnEventFire]
        public void ShowScreen(ButtonClickEvent e, SingleNode<ItemPropertiesButtonComponent> button,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            ShowScreenLeftEvent<ItemPropertiesScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(selectedItemNode.component.SelectedItem, false);
            ScheduleEvent(showScreenLeftEvent, button);
        }

        [OnEventFire]
        public void HideUpgradePart(NodeAddedEvent e, BuyableItemNode item, [JoinByScreen] [Context] ScreenNode screen) =>
            HideUpgradePart(item.Entity, screen);

        [OnEventFire]
        public void HideUpgradePart(NodeAddedEvent e, SupplyItemNode item, [Context] [JoinByScreen] ScreenNode screen) =>
            HideUpgradePart(item.Entity, screen);

        [OnEventFire]
        public void SetWeaponHeader(NodeAddedEvent e, BuyableWeaponNode item, [JoinByScreen] [Context] ScreenNode screen) =>
            screen.Entity.AddComponent(new ScreenHeaderTextComponent(
                screen.itemPropertyScreenText.PropertyWeaponText.Replace(ITEM_NAME, item.descriptionItem.name.ToUpper())));

        [OnEventFire]
        public void SetTankHeader(NodeAddedEvent e, BuyableTankNode item, [JoinByScreen] [Context] ScreenNode screen) =>
            screen.Entity.AddComponent(new ScreenHeaderTextComponent(
                screen.itemPropertyScreenText.PropertyTankText.Replace(ITEM_NAME, item.descriptionItem.name.ToUpper())));

        [OnEventFire]
        public void SetSupplyHeader(NodeAddedEvent e, BuyableSupplyNode item, [JoinByScreen] [Context] ScreenNode screen) =>
            screen.Entity.AddComponent(new ScreenHeaderTextComponent(
                screen.itemPropertyScreenText.PropertySupplyText.Replace(ITEM_NAME, item.descriptionItem.name.ToUpper())));

        void HideUpgradePart(Entity item, ScreenNode screen) {
            screen.itemPropertiesScreen.itemAttribute.ShowNextUpgradeValue = false;
            ScheduleEvent<HideItemPropertiesEvent>(screen);
            ScheduleEvent<ShowItemPropertiesEvent>(item);
            ScheduleEvent<UpdateItemPropertiesEvent>(item);
            screen.itemPropertiesScreen.itemAttribute.Hide();
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ItemPropertiesScreenComponent itemPropertiesScreen;

            public ItemPropertyScreenTextComponent itemPropertyScreenText;

            public ScreenComponent screen;
        }

        public class BuyableItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;

            public ScreenGroupComponent screenGroup;
        }

        public class SupplyItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ScreenGroupComponent screenGroup;
            public SupplyItemComponent supplyItem;

            public UserItemComponent userItem;
        }

        public class BuyableWeaponNode : Node {
            public DescriptionItemComponent descriptionItem;

            public MarketItemComponent marketItem;

            public ScreenGroupComponent screenGroup;
            public WeaponItemComponent weaponItem;
        }

        public class BuyableTankNode : Node {
            public DescriptionItemComponent descriptionItem;

            public MarketItemComponent marketItem;

            public ScreenGroupComponent screenGroup;
            public TankItemComponent tankItem;
        }

        public class BuyableSupplyNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ScreenGroupComponent screenGroup;
            public SupplyTypeComponent supplyType;
        }
    }
}