using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuySuppliesSystem : ECSSystem {
        [OnEventFire]
        public void BuySupplies(ConfirmButtonClickYesEvent e, ButtonNode buyButton,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) => NewEvent<BuySelectedSupplyItemEvent>()
            .AttachAll(selectedItem.component.SelectedItem, buyButton.Entity).Schedule();

        [OnEventFire]
        public void BuySupplies(BuySelectedSupplyItemEvent evt, ButtonNode buyButton, SupplyUserItemNode supplyUserItem,
            [JoinByMarketItem] SupplyMarketItemNode supplyMarketItem) {
            BuySuppliesClientEvent buySuppliesClientEvent = new();
            buySuppliesClientEvent.Count = buyButton.buySuppliesButton.supplyCount;
            buySuppliesClientEvent.TotalPrice = buyButton.priceButton.Price;
            ScheduleEvent(buySuppliesClientEvent, supplyMarketItem);
        }

        public class ButtonNode : Node {
            public BuySuppliesButtonComponent buySuppliesButton;
            public PriceButtonComponent priceButton;
        }

        public class SupplyUserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public SupplyItemComponent supplyItem;
            public UserItemComponent userItem;
        }

        public class SupplyMarketItemNode : Node {
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;

            public SupplyItemComponent supplyItem;
        }
    }
}