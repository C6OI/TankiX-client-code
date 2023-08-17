using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class BankCardPaymentScreenSystem : ECSSystem {
        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node) {
            Log.Error("Error making payment: " + e.ErrorCode);
            ScheduleEvent<ShowScreenLeftEvent<PaymentFailScreenComponent>>(node);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen,
            SelectedPackNode selectedPack) {
            screen.component.Receipt.SetPrice(selectedPack.goodsPrice.Price, selectedPack.goodsPrice.Currency);

            screen.component.Receipt.AddItem((string)screen.component.Receipt.Lines["amount"],
                selectedPack.xCrystalsPack.Total);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button,
            [JoinByScreen] SingleNode<BankCardPaymentScreenComponent> screen, [JoinAll] SelectedPackNode selectedPack,
            [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<AdyenPublicKeyComponent> adyenProvider,
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
            }).AttachAll(selectedPack.Entity, selectedMethod.Entity).Schedule();

            ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(screen);

            ScheduleEvent(new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.PROCEED,
                    Item = selectedPack.Entity.Id,
                    Screen = screen.component.gameObject.name,
                    Method = selectedMethod.Entity.Id
                },
                session);
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node,
            [JoinAll] SingleNode<PaymentProcessingScreenComponent> screen) =>
            ScheduleEvent<ShowScreenLeftEvent<PaymentSuccessScreenComponent>>(screen);

        public class SelectedPackNode : Node {
            public GoodsPriceComponent goodsPrice;
            public SelectedListItemComponent selectedListItem;

            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SelectedMethodNode : Node {
            public PaymentMethodComponent paymentMethod;

            public SelectedListItemComponent selectedListItem;
        }
    }
}