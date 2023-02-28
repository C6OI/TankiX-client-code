using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemOnlyInContainerUISystem : ECSSystem {
        [OnEventFire]
        public void SetMountText(NodeAddedEvent e, ScreenNode screenNode) {
            screenNode.garageItemsScreen.OnlyInContainerLabel = screenNode.garageItemsScreenText.OnlyInContainerText;
        }

        [OnEventFire]
        public void ShowOnlyInContainerUI(ListItemSelectedEvent e, InContainerMarketItemNode item, [JoinAll] ScreenNode screenNode) {
            screenNode.garageItemsScreen.OnlyInContainerLabelVisibility = item.priceItem.Price == 0 && item.xPriceItem.Price == 0;
            screenNode.garageItemsScreen.InContainerButtonVisibility = true;
        }

        [OnEventFire]
        public void HideOnlyInContainerUI(ListItemSelectedEvent e, SingleNode<UserItemComponent> item, [JoinAll] ScreenNode screenNode) {
            screenNode.garageItemsScreen.OnlyInContainerUIVisibility = false;
        }

        [OnEventFire]
        public void HideOnlyInContainerUI(ListItemSelectedEvent e, NotInContainerMarketItemNode item, [JoinAll] ScreenNode screenNode) {
            screenNode.garageItemsScreen.OnlyInContainerUIVisibility = false;
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenComponent garageItemsScreen;

            public GarageItemsScreenTextComponent garageItemsScreenText;

            public ScreenComponent screen;
        }

        public class InContainerMarketItemNode : Node {
            public ContainerContentItemGroupComponent containerContentItemGroup;
            public MarketItemComponent marketItem;

            public PriceItemComponent priceItem;

            public XPriceItemComponent xPriceItem;
        }

        [Not(typeof(ContainerContentItemGroupComponent))]
        public class NotInContainerMarketItemNode : Node {
            public MarketItemComponent marketItem;
        }
    }
}