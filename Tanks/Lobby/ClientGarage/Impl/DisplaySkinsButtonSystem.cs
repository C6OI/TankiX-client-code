using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplaySkinsButtonSystem : ECSSystem {
        [OnEventFire]
        public void UpdateSkinsButton(ListItemSelectedEvent e, Node any, [JoinAll] GarageSkinsItemsScreenNode screen,
            [JoinByScreen] [Mandatory] SingleNode<SelectedItemComponent> selectedItem) =>
            ScheduleEvent<UpdateSkinsButtonEvent>(selectedItem.component.SelectedItem);

        [OnEventFire]
        public void EnableSkinButton(NodeAddedEvent e, UserItemNode item,
            [JoinByParentGroup] ICollection<SkinMarketItemNode> skinItems, [JoinAll] GarageSkinsItemsScreenNode screen) {
            bool flag = item.parentGroup.Key == item.marketItemGroup.Key;
            screen.garageSkinsScreen.SkinsButton.gameObject.SetActive(skinItems.Count > 1 && flag);
        }

        [OnEventFire]
        public void EnableSkinButton(UpdateSkinsButtonEvent e, UserItemNode item,
            [JoinByParentGroup] ICollection<SkinMarketItemNode> skinItems, [JoinAll] GarageSkinsItemsScreenNode screen) {
            bool flag = item.parentGroup.Key == item.marketItemGroup.Key;
            screen.garageSkinsScreen.SkinsButton.gameObject.SetActive(skinItems.Count > 1 && flag);
        }

        [OnEventFire]
        public void DisableShellButton(UpdateSkinsButtonEvent e, SingleNode<MarketItemComponent> item,
            [JoinAll] GarageSkinsItemsScreenNode screen) => screen.garageSkinsScreen.SkinsButton.gameObject.SetActive(false);

        public class GarageSkinsItemsScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GarageItemsScreenComponent garageItemsScreen;

            public GarageSkinsScreenComponent garageSkinsScreen;
        }

        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public ParentGroupComponent parentGroup;

            public UserItemComponent userItem;
        }

        public class SkinMarketItemNode : Node {
            public MarketItemComponent marketItem;
            public SkinItemComponent skinItem;
        }

        public class UpdateSkinsButtonEvent : Event { }
    }
}