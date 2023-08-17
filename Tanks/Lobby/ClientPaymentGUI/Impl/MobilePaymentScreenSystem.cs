using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientPayment.main.csharp.Impl.Platbox;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen,
            SingleNode<PhoneCodesComponent> phoneCodes, UserNode user, SelectedPackNode selectedPack) {
            screen.component.PhoneCountryCode = phoneCodes.component.Codes[user.userCountry.CountryCode];
            screen.component.Receipt.SetPrice(selectedPack.goodsPrice.Price, selectedPack.goodsPrice.Currency);

            screen.component.Receipt.AddItem((string)screen.component.Receipt.Lines["amount"],
                selectedPack.xCrystalsPack.Total);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button,
            [JoinByScreen] SingleNode<MobilePaymentScreenComponent> screen, [JoinAll] SelectedPackNode selectedPack,
            [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<ClientSessionComponent> session) {
            string text = screen.component.PhoneCountryCode + screen.component.PhoneNumber.Replace(" ", string.Empty);
            MobilePaymentDataComponent mobilePaymentDataComponent = new();
            mobilePaymentDataComponent.PhoneNumber = text;
            MobilePaymentDataComponent component = mobilePaymentDataComponent;
            CreateEntity("MobilePayment").AddComponent(component);

            NewEvent(new PlatBoxBuyGoodsEvent {
                Phone = text
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
        [Mandatory]
        public void Success(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node,
            [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment) {
            mobilePayment.component.TransactionId = e.TransactionId;
            ScheduleEvent<ShowScreenLeftEvent<MobilePaymentCheckoutScreenComponent>>(node);
        }

        public class SelectedPackNode : Node {
            public GoodsPriceComponent goodsPrice;
            public SelectedListItemComponent selectedListItem;

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