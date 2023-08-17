using System.Collections.Generic;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class GoodsSelectionScreenSystem : ECSSystem {
        [OnEventFire]
        public void RequestGoods(NodeAddedEvent e, GoodsSelectionScreenNode screen, UserNode user) =>
            ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);

        [OnEventFire]
        public void InitScreen(PaymentSectionLoadedEvent e, Node node, [JoinAll] GoodsSelectionScreenNode screen,
            [JoinAll] ICollection<PackNode> packs) => CreatePacks(packs, screen.goodsSelectionScreen.List);

        void CreatePacks(ICollection<PackNode> packs, IUIList list) {
            List<PackNode> list2 = packs.ToList();
            list2.Sort((a, b) => a.goodsPrice.Price.CompareTo(b.goodsPrice.Price));

            foreach (PackNode item in list2) {
                list.AddItem(item.Entity);
            }
        }

        [OnEventFire]
        public void OpenSystemsSelectionScreen(ListItemSelectedEvent e, SelectedPackNode selectedGoods,
            [JoinAll] GoodsSelectionScreenNode screen, [JoinAll] SingleNode<ClientSessionComponent> session) {
            Entity entity = selectedGoods.Entity;
            ShowScreenLeftEvent<MethodSelectionScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, false);
            ScheduleEvent(showScreenLeftEvent, entity);

            ScheduleEvent(new PaymentStatisticsEvent {
                    Screen = screen.goodsSelectionScreen.gameObject.name,
                    Action = PaymentStatisticsAction.ITEM_SELECT,
                    Item = entity.Id
                },
                session);
        }

        public class GoodsSelectionScreenNode : Node {
            public GoodsSelectionScreenComponent goodsSelectionScreen;
        }

        public class PackNode : Node {
            public GoodsPriceComponent goodsPrice;
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SelectedPackNode : PackNode {
            public SelectedListItemComponent selectedListItem;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserCountryComponent userCountry;
        }
    }
}