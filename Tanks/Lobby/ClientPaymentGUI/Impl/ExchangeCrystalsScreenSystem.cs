using System.Collections.Generic;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ExchangeCrystalsScreenSystem : ECSSystem {
        [OnEventFire]
        public void GoToExchange(ButtonClickEvent e, SingleNode<UserMoneyIndicatorComponent> button,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent<GoToExchangeEvent>(user);

        [OnEventFire]
        public void GoToExchange(GoToExchangeEvent e, SingleNode<SelfUserComponent> user,
            [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<ScreenComponent> screen) {
            ScheduleEvent<ShowScreenLeftEvent<ExchangeCrystalsScreenComponent>>(user);

            ScheduleEvent(new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.OPEN_EXCHANGE,
                    Screen = screen.component.name
                },
                session);
        }

        [OnEventFire]
        public void RequestPacks(NodeAddedEvent e, ScreenNode screen, SingleNode<SelfUserComponent> user) =>
            ScheduleEvent<OpenExchangePaymentSectionEvent>(user);

        [OnEventFire]
        public void InitScreen(PaymentSectionLoadedEvent e, Node node, [JoinAll] ScreenNode screen,
            [JoinAll] ICollection<PackNode> packs) => CreatePacks(packs, screen.exchangeCrystalsScreen.List);

        [OnEventFire]
        public void Exchange(ListItemSelectedEvent e, SelectedPackNode selectedGoods, [JoinAll] ScreenNode screen,
            [JoinAll] UserNode user, [JoinAll] SingleNode<DialogsComponent> dialogs) {
            if (user.userXCrystals.Money < selectedGoods.goodsXPrice.Price) {
                dialogs.component.Get<NotEnoughCrystalsWindow>().ShowForXCrystals(user.Entity,
                    screen.Entity,
                    selectedGoods.goodsXPrice.Price - user.userXCrystals.Money);
            } else {
                dialogs.component.Get<ExchangeConfirmationWindow>().Show(user.Entity, selectedGoods.Entity, screen.Entity);
            }
        }

        void CreatePacks(ICollection<PackNode> packs, IUIList list) {
            List<PackNode> list2 = packs.ToList();
            list2.Sort((a, b) => a.goodsXPrice.Price.CompareTo(b.goodsXPrice.Price));

            foreach (PackNode item in list2) {
                list.AddItem(item.Entity);
            }
        }

        public class PackNode : Node {
            public GameCurrencyPackComponent gameCurrencyPack;

            public GoodsXPriceComponent goodsXPrice;
        }

        public class SelectedPackNode : PackNode {
            public SelectedListItemComponent selectedListItem;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserXCrystalsComponent userXCrystals;
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ExchangeCrystalsScreenComponent exchangeCrystalsScreen;
        }
    }
}