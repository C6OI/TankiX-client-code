using System.Collections.Generic;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;
using OrderItemComponent = Tanks.Lobby.ClientPayment.Impl.OrderItemComponent;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class DealsUISystem : ECSSystem {
        [OnEventFire]
        public void AddPromo(NodeAddedEvent e, SingleNode<DealsUIComponent> deals, [JoinAll] Optional<SingleNode<GiftPromoUIDataComponent>> promo,
            [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            deals.component.shopDialogs = dialogs.component.Get<ShopDialogs>();

            if (promo.IsPresent()) {
                deals.component.AddPromo(promo.Get().component.PromoKey);
            } else {
                deals.component.RemovePromo();
            }
        }

        [OnEventFire]
        public void AddPromo(NodeAddedEvent e, SingleNode<GiftPromoUIDataComponent> promo, [JoinAll] SingleNode<DealsUIComponent> deals) {
            deals.component.AddPromo(promo.component.PromoKey);
        }

        [OnEventFire]
        public void RemovePromo(NodeRemoveEvent e, SingleNode<GiftPromoUIDataComponent> promo, [JoinAll] SingleNode<DealsUIComponent> deals,
            [JoinAll] SingleNode<GiftsPromoComponent> promoObj) {
            deals.component.RemovePromo();
        }

        [OnEventFire]
        public void AddSpecialOffer(NodeAddedEvent e, SingleNode<DealsUIComponent> deals, [Combine] SpecialOfferNode offer,
            [JoinBy(typeof(SpecialOfferGroupComponent))]
            PersonalSpecialOfferPropertyNode personalOfferProperty) {
            if (offer.Entity.HasComponent<LeagueFirstEntranceSpecialOfferComponent>()) {
                GameObject gameObject = deals.component.leagueSpecialOfferPrefab.gameObject;
                SpecialOfferContent specialOfferContent = deals.component.AddSpecialOffer(offer.Entity, gameObject);
                List<SpecialOfferItem> list = new();
                CountableItemsPackComponent component = offer.Entity.GetComponent<CountableItemsPackComponent>();

                foreach (KeyValuePair<long, int> item in component.Pack) {
                    long key = item.Key;
                    Entity entity = Flow.Current.EntityRegistry.GetEntity(key);
                    int value = item.Value;
                    string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                    string name = entity.GetComponent<DescriptionItemComponent>().Name;
                    list.Add(new SpecialOfferItem(value, spriteUid, name));
                }

                int worthItPercent = offer.Entity.GetComponent<LeagueFirstEntranceSpecialOfferComponent>().WorthItPercent;
                specialOfferContent.GetComponent<LeagueSpecialOfferComponent>().ShowOfferItems(list, worthItPercent);
            } else {
                deals.component.AddSpecialOffer(offer.Entity);
            }
        }

        [OnEventFire]
        public void RemoveSpecialOfferNode(NodeRemoveEvent e, PersonalSpecialOfferPropertyNode node, [JoinBy(typeof(SpecialOfferGroupComponent))] SpecialOfferNode offer,
            [JoinAll] SingleNode<DealsUIComponent> deals) {
            deals.component.RemoveSpecialOffer(offer.Entity);
        }

        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<DealsUIComponent> deals) {
            deals.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<DealsUIComponent> deals) {
            deals.component.Clear();
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<DealsUIComponent> deals) {
            Log.Debug("GoToUrl");
            deals.component.HandleGoToLink();
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<DealsUIComponent> deals) {
            Log.Debug("SuccessMobile");
            deals.component.HandleSuccessMobile(e.TransactionId);
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<DealsUIComponent> deals) {
            Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<DealsUIComponent> deals) {
            Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<DealsUIComponent> deals) {
            Log.Debug("Success");
            deals.component.HandleSuccess();
        }

        [OnEventFire]
        public void AddmarketItemSale(NodeAddedEvent e, [Combine] MarketItemSaleNode marketItemSaleNode, [Context] SingleNode<DealsUIComponent> deals) {
            deals.component.AddMarketItem(marketItemSaleNode.Entity);
        }

        [Not(typeof(LegendaryTankSpecialOfferComponent))]
        public class SpecialOfferNode : Node {
            public GoodsPriceComponent goodsPrice;

            public ItemsPackFromConfigComponent itemsPackFromConfig;

            public OrderItemComponent orderItem;
            public SpecialOfferComponent specialOffer;

            public SpecialOfferDurationComponent specialOfferDuration;

            public SpecialOfferEndTimeComponent specialOfferEndTime;
        }

        public class PersonalSpecialOfferPropertyNode : Node {
            public OrderItemComponent orderItem;
            public PersonalSpecialOfferPropertiesComponent personalSpecialOfferProperties;

            public SpecialOfferGroupComponent specialOfferGroup;

            public SpecialOfferVisibleComponent specialOfferVisible;

            public UserGroupComponent userGroup;
        }

        public class MarketItemSaleNode : Node {
            public MarketItemSaleComponent marketItemSale;
        }
    }
}