using System.Collections.Generic;
using System.Linq;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MethodSelectionScreenSystem : ECSSystem {
        [OnEventFire]
        public void AddMethodsButtons(NodeAddedEvent e, ScreenNode screen, [JoinAll] SelectedGoodsNode goods, [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods) {
            List<SingleNode<PaymentMethodComponent>> list = methods.ToList();

            if (goods.Entity.HasComponent<SpecialOfferComponent>()) {
                list.RemoveAll((SingleNode<PaymentMethodComponent> x) => x.component.IsTerminal);
            }

            List<SingleNode<PaymentMethodComponent>> list2 = new(list);

            list2.Sort((SingleNode<PaymentMethodComponent> p1, SingleNode<PaymentMethodComponent> p2) =>
                string.CompareOrdinal(p1.component.ProviderName + p1.component.MethodName, p2.component.ProviderName + p2.component.MethodName));

            foreach (SingleNode<PaymentMethodComponent> item in list2) {
                screen.methodSelectionScreen.List.AddItem(item.Entity);
            }
        }

        [OnEventComplete]
        public void BuyGoods(ListItemSelectedEvent e, SelectedMethodNode method, [JoinAll] SelectedGoodsNode goods, [JoinAll] ScreenNode screen,
            [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] ICollection<SelectedMethodNode> methods) {
            if (methods.Count > 1) {
                foreach (SelectedMethodNode method2 in methods) {
                    if (method2.Entity != method.Entity) {
                        method2.Entity.RemoveComponent<SelectedListItemComponent>();
                    }
                }
            }

            ScheduleEvent(new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.MODE_SELECT,
                Item = goods.Entity.Id,
                Screen = screen.methodSelectionScreen.gameObject.name,
                Method = method.Entity.Id
            }, session);

            if (method.paymentMethod.MethodName == PaymentMethodNames.CREDIT_CARD && method.paymentMethod.ProviderName == "adyen") {
                ScheduleEvent<ShowScreenLeftEvent<BankCardPaymentScreenComponent>>(screen);
                return;
            }

            if (method.paymentMethod.MethodName == PaymentMethodNames.MOBILE) {
                ScheduleEvent<ShowScreenLeftEvent<MobilePaymentScreenComponent>>(screen);
                return;
            }

            if (method.paymentMethod.MethodName == PaymentMethodNames.QIWI_WALLET && method.paymentMethod.ProviderName == "qiwi") {
                ScheduleEvent<ShowScreenLeftEvent<QiwiWalletScreenComponent>>(screen);
                return;
            }

            ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(method);
            NewEvent<ProceedToExternalPaymentEvent>().AttachAll(method, goods).Schedule();

            ScheduleEvent(new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.PROCEED,
                Item = goods.Entity.Id,
                Screen = screen.methodSelectionScreen.gameObject.name,
                Method = method.Entity.Id
            }, session);
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

            public GoodsPriceComponent goodsPrice;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedXCrystalsPackNode : SelectedGoodsNode {
            public XCrystalsPackComponent xcrystalsPack;
        }
    }
}