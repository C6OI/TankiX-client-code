using System;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentCheckoutScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen, [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment,
            [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod) {
            double price = selectedGood.goodsPrice.Price;

            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price)
                        : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);

            screen.component.SetPrice(price, selectedGood.goodsPrice.Currency);
            DeleteEntity(mobilePayment.Entity);
            screen.component.SetTransactionNumber(mobilePayment.component.TransactionId);
            screen.component.SetPhoneNumber(mobilePayment.component.PhoneNumber);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen, [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment,
            [JoinAll] SelectedPackNode selectedPack, [JoinAll] SelectedMethodNode selectedMethod) {
            long num = selectedPack.xCrystalsPack.Amount;

            if (!selectedPack.Entity.HasComponent<SpecialOfferComponent>()) {
                num = (long)Math.Round(selectedPack.goods.SaleState.AmountMultiplier * num);
            }

            screen.component.SetCrystalsAmount(num + selectedPack.xCrystalsPack.Bonus);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen, [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment,
            [JoinAll] SelectedOfferNode selectedOffer) { }

        public class SelectedGoodNode : Node {
            public GoodsComponent goods;

            public GoodsPriceComponent goodsPrice;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedOfferNode : SelectedGoodNode {
            public ReceiptTextComponent receiptText;
            public SpecialOfferComponent specialOffer;
        }

        public class SelectedPackNode : SelectedGoodNode {
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SelectedMethodNode : Node {
            public PaymentMethodComponent paymentMethod;

            public SelectedListItemComponent selectedListItem;
        }
    }
}