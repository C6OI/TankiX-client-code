using System;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientPayment.main.csharp.Impl.Platbox;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen, SingleNode<PhoneCodesComponent> phoneCodes, UserNode user, SelectedGoodNode selectedGood,
            [JoinAll] SelectedMethodNode selectedMethod) {
            screen.component.PhoneCountryCode = phoneCodes.component.Codes[user.userCountry.CountryCode];
            double price = selectedGood.goodsPrice.Price;

            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price)
                        : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);

            screen.component.Receipt.SetPrice(price, selectedGood.goodsPrice.Currency);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen, SingleNode<PhoneCodesComponent> phoneCodes, UserNode user, SelectedPackNode selectedPack,
            [JoinAll] SelectedMethodNode selectedMethod) {
            long num = selectedPack.xCrystalsPack.Amount;

            if (!selectedPack.Entity.HasComponent<SpecialOfferComponent>()) {
                num = (long)Math.Round(selectedPack.goods.SaleState.AmountMultiplier * num);
            }

            screen.component.Receipt.AddItem((string)screen.component.Receipt.Lines["amount"], num + selectedPack.xCrystalsPack.Bonus);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen, SelectedOfferNode selectedOffer) {
            screen.component.Receipt.AddSpecialOfferText(selectedOffer.receiptText.Text);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<MobilePaymentScreenComponent> screen,
            [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<ClientSessionComponent> session) {
            string text = screen.component.PhoneCountryCode + screen.component.PhoneNumber.Replace(" ", string.Empty);
            MobilePaymentDataComponent mobilePaymentDataComponent = new();
            mobilePaymentDataComponent.PhoneNumber = text;
            MobilePaymentDataComponent component = mobilePaymentDataComponent;
            CreateEntity("MobilePayment").AddComponent(component);

            NewEvent(new PlatBoxBuyGoodsEvent {
                Phone = text
            }).AttachAll(selectedGood.Entity, selectedMethod.Entity).Schedule();

            ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(screen);

            ScheduleEvent(new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.PROCEED,
                Item = selectedGood.Entity.Id,
                Screen = screen.component.gameObject.name,
                Method = selectedMethod.Entity.Id
            }, session);
        }

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

        public class UserNode : Node {
            public SelfUserComponent selfUser;
            public UserCountryComponent userCountry;
        }
    }
}