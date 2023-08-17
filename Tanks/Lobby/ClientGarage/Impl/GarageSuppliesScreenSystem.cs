using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageSuppliesScreenSystem : ECSSystem {
        [OnEventComplete]
        public void SetItemCount(NodeAddedEvent e, SupplyListItemNode listItemNode) => UpdateCount(listItemNode);

        [OnEventFire]
        public void UpdateSupplyCount(SupplyCountChangedEvent e, SupplyListItemNode listItemNode) =>
            UpdateCount(listItemNode);

        void UpdateCount(SupplyListItemNode listItemNode) =>
            listItemNode.garageListItemContent.Count.text = listItemNode.supplyCount.Count.ToString();

        [OnEventFire]
        public void ShowBuyButton(ListItemSelectedEvent e, SupplyListItemNode itemNode,
            [JoinByMarketItem] SupplyMarketItemNode supplyMarketItem, [Combine] [JoinAll] BuyButtonNode buttonNode) {
            BuySuppliesButtonComponent buySuppliesButton = buttonNode.buySuppliesButton;
            int supplyCount = buySuppliesButton.supplyCount;

            buttonNode.confirmButton.ButtonText = buttonNode.buyButtonConfirmWithPriceLocalizedText.BuyText +
                                                  " " +
                                                  supplyCount +
                                                  " " +
                                                  buttonNode.buyButtonConfirmWithPriceLocalizedText.ForText;

            ScheduleEvent(new SetPriceEvent {
                    Price = supplyMarketItem.priceItem.Price * supplyCount
                },
                buttonNode);

            buttonNode.priceButton.Price = supplyMarketItem.priceItem.Price * supplyCount;
        }

        public class SupplyMarketItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public PriceItemComponent priceItem;

            public SupplyItemComponent supplyItem;
        }

        public class SupplyListItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageListItemContentComponent garageListItemContent;

            public MarketItemGroupComponent marketItemGroup;

            public SupplyCountComponent supplyCount;
        }

        public class BuyButtonNode : Node {
            public BuyButtonComponent buyButton;

            public BuyButtonConfirmWithPriceLocalizedTextComponent buyButtonConfirmWithPriceLocalizedText;

            public BuySuppliesButtonComponent buySuppliesButton;

            public ConfirmButtonComponent confirmButton;
            public PriceButtonComponent priceButton;
        }
    }
}