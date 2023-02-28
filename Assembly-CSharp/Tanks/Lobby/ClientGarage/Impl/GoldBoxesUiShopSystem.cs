using System;
using System.Collections;
using System.Collections.Generic;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBoxesUiShopSystem : ECSSystem {
        [OnEventFire]
        public void CreatePacks(NodeAddedEvent e, SingleNode<GoldBoxesShopTabComponent> shopNode, [JoinAll] ICollection<GoldBoxOfferNode> goods) {
            List<GoldBoxOfferNode> list = BuildList(goods);
            list.Sort(new GoldBoxNodeComparer());

            foreach (GoldBoxOfferNode item in list) {
                GoldBoxesPackComponent component = Object.Instantiate(shopNode.component.PackPrefab, shopNode.component.PackContainer).GetComponent<GoldBoxesPackComponent>();
                FillPack(component, item);
            }
        }

        [OnEventFire]
        public void DestroyPacks(NodeRemoveEvent e, SingleNode<GoldBoxesShopTabComponent> shopNode) {
            IEnumerator enumerator = shopNode.component.PackContainer.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    Object.Destroy(transform.gameObject);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        List<GoldBoxOfferNode> BuildList(ICollection<GoldBoxOfferNode> goods) => new(goods);

        void FillPack(GoldBoxesPackComponent pack, GoldBoxOfferNode packNode) {
            pack.CardName = packNode.specialOfferContentLocalization.Title;
            pack.SpriteUid = packNode.specialOfferScreenLocalization.SpriteUid;
            pack.Discount = packNode.specialOfferContent.SalePercent;
            pack.HitMarkEnabled = packNode.specialOfferContent.HighlightTitle;
            pack.BoxCount = packNode.goldBonusOffer.Count;
            pack.Price = string.Format("{0:0.00} {1}", packNode.goodsPrice.Price, packNode.goodsPrice.Currency);
            pack.GoodsEntity = packNode.Entity;
        }

        [OnEventFire]
        public void GetCounter(NodeAddedEvent e, SingleNode<GoldBoxesShopTabComponent> shopNode, [JoinAll] GoldBoxItemNode gold) {
            shopNode.component.UserBoxCount.text = gold.userItemCounter.Count.ToString();
        }

        [OnEventFire]
        public void RefreshCounter(TryToShowNotificationEvent e, Node any, [JoinAll] GoldBoxItemNode gold, [JoinAll] SingleNode<GoldBoxesShopTabComponent> shopNode) {
            shopNode.component.UserBoxCount.text = gold.userItemCounter.Count.ToString();
        }

        public class GoldBoxOfferNode : Node {
            public GoldBonusOfferComponent goldBonusOffer;

            public GoodsPriceComponent goodsPrice;

            public SpecialOfferContentComponent specialOfferContent;

            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;

            public SpecialOfferScreenLocalizationComponent specialOfferScreenLocalization;
        }

        class GoldBoxNodeComparer : IComparer<GoldBoxOfferNode> {
            public int Compare(GoldBoxOfferNode a, GoldBoxOfferNode b) => a.goldBonusOffer.Count.CompareTo(b.goldBonusOffer.Count);
        }

        public class GoldBoxItemNode : Node {
            public GoldBonusItemComponent goldBonusItem;

            public UserGroupComponent userGroup;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }
    }
}