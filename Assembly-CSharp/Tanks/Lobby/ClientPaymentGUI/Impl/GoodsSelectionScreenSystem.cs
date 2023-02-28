using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.Impl;
using OrderItemComponent = Tanks.Lobby.ClientPayment.Impl.OrderItemComponent;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class GoodsSelectionScreenSystem : ECSSystem {
        [OnEventFire]
        public void ClearOffers(PaymentSectionLoadedEvent e, Node node, [JoinAll] GoodsSelectionScreenNode screen) {
            screen.goodsSelectionScreen.SpecialOfferDataProvider.ClearItems();
            screen.goodsSelectionScreen.XCrystalsDataProvider.ClearItems();
        }

        [OnEventFire]
        public void InitiateUpdateList(PaymentSectionLoadedEvent e, SingleNode<SelfUserComponent> user, Optional<SingleNode<SteamComponent>> steamOptional) {
            if (!steamOptional.IsPresent() || !string.IsNullOrEmpty(SteamComponent.Ticket)) {
                ScheduleEvent<UpdateListEvent>(user.Entity);
            }
        }

        [OnEventFire]
        public void SetOfferTime(NodeAddedEvent e, SingleNode<SpecialOfferRemainingTimeComponent> remainNode) {
            remainNode.component.EndDate = Date.Now.AddSeconds(remainNode.component.Remain);
            SpecialOfferEndTimeComponent specialOfferEndTimeComponent = new();
            specialOfferEndTimeComponent.EndDate = remainNode.component.EndDate;
            SpecialOfferEndTimeComponent component = specialOfferEndTimeComponent;
            remainNode.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void UnsetOfferTime(NodeRemoveEvent e, SingleNode<SpecialOfferRemainingTimeComponent> remainNode) {
            if (remainNode.Entity.HasComponent<SpecialOfferEndTimeComponent>()) {
                remainNode.Entity.RemoveComponent<SpecialOfferEndTimeComponent>();
            }
        }

        [OnEventFire]
        public void SetOfferTime(NodeAddedEvent e, ClientOfferNode specialOffer, [JoinBy(typeof(SpecialOfferGroupComponent))] [Context] ClientPersonalOfferProperties personalProperty) {
            SpecialOfferEndTimeComponent specialOfferEndTimeComponent = new();
            specialOfferEndTimeComponent.EndDate = personalProperty.specialOfferEndTime.EndDate;
            SpecialOfferEndTimeComponent component = specialOfferEndTimeComponent;
            specialOffer.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetOrderIndexToOffer(UpdateListEvent e, SingleNode<SelfUserComponent> user, [JoinByUser] [Combine] PersonalSpecialOfferPropertyNode personalOfferProperty,
            [JoinBy(typeof(SpecialOfferGroupComponent))]
            SpecialOfferNode offer) {
            personalOfferProperty.orderItem.Index = offer.orderItem.Index;
        }

        [OnEventComplete]
        public void FilterOffers(UpdateListEvent e, SingleNode<SelfUserComponent> user, [JoinByUser] ICollection<PersonalSpecialOfferPropertyNode> personalOfferProperties) {
            List<PersonalSpecialOfferPropertyNode> list = personalOfferProperties.ToList();
            list.Sort((PersonalSpecialOfferPropertyNode a, PersonalSpecialOfferPropertyNode b) => a.orderItem.Index.CompareTo(b.orderItem.Index));

            foreach (PersonalSpecialOfferPropertyNode item in list) {
                ScheduleEvent<CollectOfferEvent>(item);
            }
        }

        [OnEventFire]
        public void RegistratePaymentIntentComponent(NodeAddedEvent e, SingleNode<PaymentIntentComponent> paymentIntent) { }

        [OnEventFire]
        public void AddOffer(CollectOfferEvent e, PersonalSpecialOfferPropertyNode personalOfferProperty, [JoinBy(typeof(SpecialOfferGroupComponent))] SpecialOfferNode offer,
            [JoinAll] GoodsSelectionScreenNode screen) {
            StringBuilder stringBuilder = new();

            if (offer.itemsPackFromConfig.Pack.Count > 0) {
                int num = 0;
                stringBuilder.Append("* —");
                bool flag = true;

                foreach (long item in offer.itemsPackFromConfig.Pack) {
                    ItemInMarketRequestEvent itemInMarketRequestEvent = new();
                    ScheduleEvent(itemInMarketRequestEvent, offer);

                    if (itemInMarketRequestEvent.marketItems.ContainsKey(item)) {
                        if (!flag) {
                            stringBuilder.Append(", ");
                        }

                        flag = false;
                        stringBuilder.Append(itemInMarketRequestEvent.marketItems[item]);
                        num++;
                    }
                }

                if (num == 0) {
                    stringBuilder.Append(screen.goodsSelectionScreen.SpecialOfferEmptyRewardMessage);
                }
            }

            if (offer.specialOfferDuration.OneShot && personalOfferProperty.Entity.HasComponent(typeof(PaymentIntentComponent))) {
                if (stringBuilder.Length > 0) {
                    stringBuilder.Append("\n");
                }

                stringBuilder.Append(screen.goodsSelectionScreen.SpecialOfferOneShotMessage);
            }

            screen.goodsSelectionScreen.SpecialOfferDataProvider.AddItem(offer.Entity, stringBuilder.ToString());
        }

        [OnEventFire]
        public void InitScreen(UpdateListEvent e, Node node, [JoinAll] GoodsSelectionScreenNode screen, [JoinAll] ICollection<PackNode> packs) {
            CreatePacks(packs, screen.goodsSelectionScreen.XCrystalsDataProvider);
        }

        void CreatePacks(ICollection<PackNode> packs, IUIList list) {
            List<PackNode> list2 = packs.ToList();
            list2.Sort((PackNode a, PackNode b) => a.goodsPrice.Price.CompareTo(b.goodsPrice.Price));

            foreach (PackNode item in list2) {
                list.AddItem(item.Entity);
            }
        }

        [OnEventFire]
        public void OpenSystemsSelectionScreen(ListItemSelectedEvent e, SelectedNode selectedGoods, [JoinAll] GoodsSelectionScreenNode screen,
            [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] Optional<SingleNode<SteamComponent>> steam) {
            Entity entity = selectedGoods.Entity;

            if (steam.IsPresent()) {
                if (string.IsNullOrEmpty(SteamComponent.Ticket)) {
                    ShowScreenLeftEvent<MethodSelectionScreenComponent> showScreenLeftEvent = new();
                    showScreenLeftEvent.SetContext(entity, false);
                } else {
                    ScheduleEvent<SteamBuyGoodsEvent>(entity);
                }
            } else {
                ShowScreenEvent showScreenEvent = new ShowScreenLeftEvent<MethodSelectionScreenComponent>();
                showScreenEvent.SetContext(entity, false);
                ScheduleEvent(showScreenEvent, entity);
            }

            ScheduleEvent(new PaymentStatisticsEvent {
                Screen = screen.goodsSelectionScreen.gameObject.name,
                Action = PaymentStatisticsAction.ITEM_SELECT,
                Item = entity.Id
            }, session);
        }

        [OnEventFire]
        public void OpenSystemsSelectionScreen(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<FirstPurchaseConfirmScreenComponent> screen,
            [JoinAll] SelectedNode selectedGoods, [JoinAll] SingleNode<ClientSessionComponent> session) {
            Entity entity = selectedGoods.Entity;
            ShowScreenEvent showScreenEvent = new ShowScreenLeftEvent<MethodSelectionScreenComponent>();
            showScreenEvent.SetContext(entity, false);
            ScheduleEvent(showScreenEvent, entity);

            ScheduleEvent(new PaymentStatisticsEvent {
                Screen = screen.component.gameObject.name,
                Action = PaymentStatisticsAction.CONFIRMED_ONE_TIME_OFFER,
                Item = entity.Id
            }, session);
        }

        [OnEventFire]
        public void InitConfirmation(NodeAddedEvent e, SingleNode<FirstPurchaseConfirmScreenComponent> screen, SelectedNode selectedGoods, UserNode user) {
            NewEvent<CalculateCompensationRequestEvent>().AttachAll(selectedGoods, user).Schedule();
        }

        [OnEventFire]
        public void InitConfirmation(CalculateCompensationResponseEvent e, SelectedNode selectedGoods, UserNode user, [JoinAll] SingleNode<FirstPurchaseConfirmScreenComponent> screen) {
            screen.component.Compensation = e.Amount;
        }

        public class GoodsSelectionScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GoodsSelectionScreenComponent goodsSelectionScreen;

            public XCrystalsSaleEndTimerComponent xCrystalsSaleEndTimer;
        }

        [Not(typeof(SpecialOfferComponent))]
        public class PackNode : Node {
            public GoodsPriceComponent goodsPrice;
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SpecialOfferNode : Node {
            public GoodsPriceComponent goodsPrice;

            public ItemsPackFromConfigComponent itemsPackFromConfig;

            public OrderItemComponent orderItem;
            public SpecialOfferComponent specialOffer;

            public SpecialOfferDurationComponent specialOfferDuration;
        }

        public class SelectedNode : Node {
            public GoodsPriceComponent goodsPrice;
            public SelectedListItemComponent selectedListItem;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserCountryComponent userCountry;
        }

        [Not(typeof(SpecialOfferEndTimeComponent))]
        public class ClientOfferNode : Node {
            public SpecialOfferComponent specialOffer;
            public SpecialOfferGroupComponent specialOfferGroup;
        }

        public class ClientPersonalOfferProperties : Node {
            public SpecialOfferEndTimeComponent specialOfferEndTime;

            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferRemainingTimeComponent specialOfferRemainingTime;
        }

        public class PersonalSpecialOfferPropertyNode : Node {
            public OrderItemComponent orderItem;
            public PersonalSpecialOfferPropertiesComponent personalSpecialOfferProperties;

            public SpecialOfferGroupComponent specialOfferGroup;

            public SpecialOfferVisibleComponent specialOfferVisible;

            public UserGroupComponent userGroup;
        }

        public class CollectOfferEvent : Event { }

        public class UpdateListEvent : Event { }
    }
}