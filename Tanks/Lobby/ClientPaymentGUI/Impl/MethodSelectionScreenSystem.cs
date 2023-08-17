using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MethodSelectionScreenSystem : ECSSystem {
        [OnEventFire]
        public void AddMethodsButtons(NodeAddedEvent e, ScreenNode screen,
            [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods) {
            foreach (SingleNode<PaymentMethodComponent> method in methods) {
                screen.methodSelectionScreen.List.AddItem(method.Entity);
            }
        }

        [OnEventComplete]
        public void BuyGoods(ListItemSelectedEvent e, SelectedMethodNode method, [JoinAll] SelectedGoodsNode goods,
            [JoinAll] ScreenNode screen, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent(new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.MODE_SELECT,
                    Item = goods.Entity.Id,
                    Screen = screen.methodSelectionScreen.gameObject.name,
                    Method = method.Entity.Id
                },
                session);

            if (method.paymentMethod.MethodName == PaymentMethodNames.CREDIT_CARD &&
                method.paymentMethod.ProviderName == "adyen") {
                ScheduleEvent<ShowScreenLeftEvent<BankCardPaymentScreenComponent>>(screen);
                return;
            }

            if (method.paymentMethod.MethodName == PaymentMethodNames.MOBILE) {
                ScheduleEvent<ShowScreenLeftEvent<MobilePaymentScreenComponent>>(screen);
                return;
            }

            ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(method);
            NewEvent<ProceedToExternalPaymentEvent>().AttachAll(method, goods).Schedule();

            ScheduleEvent(new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.PROCEED,
                    Item = goods.Entity.Id,
                    Screen = screen.methodSelectionScreen.gameObject.name,
                    Method = method.Entity.Id
                },
                session);
        }

        public class ScreenNode : Node {
            public MethodSelectionScreenComponent methodSelectionScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class SelectedMethodNode : Node {
            public PaymentMethodComponent paymentMethod;

            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedGoodsNode : Node {
            public GoodsComponent goods;
            public SelectedListItemComponent selectedListItem;
        }
    }
}