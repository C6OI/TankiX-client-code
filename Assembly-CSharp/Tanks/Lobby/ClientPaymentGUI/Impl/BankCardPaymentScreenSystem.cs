using System;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class BankCardPaymentScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitScreenPrice(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen, SelectedGoodNode selectedGood) {
            double price = selectedGood.goodsPrice.Price;

            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price)
                        : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);

            screen.component.Receipt.SetPrice(price, selectedGood.goodsPrice.Currency);
        }

        [OnEventFire]
        public void InitScreenXCrystalsPack(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen, SelectedPackNode selectedGood) {
            long num = selectedGood.xCrystalsPack.Amount;

            if (selectedGood.Entity.HasComponent<SpecialOfferComponent>()) {
                num = (long)Math.Round(selectedGood.goods.SaleState.AmountMultiplier * num);
            }

            screen.component.Receipt.AddItem((string)screen.component.Receipt.Lines["amount"], num + selectedGood.xCrystalsPack.Bonus);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen, SelectedOfferNode selectedOffer) {
            screen.component.Receipt.AddSpecialOfferText(selectedOffer.receiptText.Text);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<BankCardPaymentScreenComponent> screen,
            [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<AdyenPublicKeyComponent> adyenProvider,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            Encrypter encrypter = new(adyenProvider.component.PublicKey);
            BankCardPaymentScreenComponent component = screen.component;

            string encrypedCard = encrypter.Encrypt(new Card {
                number = component.Number.Replace(" ", string.Empty),
                expiryMonth = int.Parse(component.MM).ToString(),
                expiryYear = "20" + component.YY,
                holderName = component.CardHolder,
                cvc = component.CVC
            }.ToString());

            NewEvent(new AdyenBuyGoodsByCardEvent {
                EncrypedCard = encrypedCard
            }).AttachAll(selectedGood.Entity, selectedMethod.Entity).Schedule();

            ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(screen);

            ScheduleEvent(new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.PROCEED,
                Item = selectedGood.Entity.Id,
                Screen = screen.component.gameObject.name,
                Method = selectedMethod.Entity.Id
            }, session);
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<PaymentProcessingScreenComponent> screen) {
            ScheduleEvent<ShowScreenLeftEvent<PaymentSuccessScreenComponent>>(screen);
        }

        public class SelectedGoodNode : Node {
            public GoodsComponent goods;

            public GoodsPriceComponent goodsPrice;

            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedPackNode : SelectedGoodNode {
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SelectedOfferNode : SelectedGoodNode {
            public ReceiptTextComponent receiptText;
            public SpecialOfferComponent specialOffer;
        }

        public class SelectedMethodNode : Node {
            public PaymentMethodComponent paymentMethod;

            public SelectedListItemComponent selectedListItem;
        }
    }
}