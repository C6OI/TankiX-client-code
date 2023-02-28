using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    internal class GarageSaleSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void UpdateGaragePrice(NodeAddedEvent e, MarketItemNode marketItem) {
            NewEvent<UpdateGaragePriceEvent>().Attach(marketItem).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateGaragePrice(NodeAddedEvent e, UserItemNode userItem, [JoinByMarketItem] MarketItemNode marketItem) {
            NewEvent<UpdateGaragePriceEvent>().Attach(marketItem).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateFirstBuySale(UpdateGaragePriceEvent e, MarketItemWithFirstBuySaleNode marketItem, [JoinByMarketItem] ICollection<UserItemNode> userItems) {
            List<UserItemNode> list = userItems.Where(i => i.userGroup.Key == SelfUserComponent.SelfUser.Id).ToList();
            int personalSalePercent = list.Count == 0 ? marketItem.firstBuySale.SalePercent : 0;
            GarageItemsRegistry.GetItem<GarageItem>(marketItem.Entity).PersonalSalePercent = personalSalePercent;
        }

        [OnEventFire]
        public void UpdateIncreasePrice(UpdateGaragePriceEvent e, MarketItemWithAutoIncreasePriceNode marketItem, [JoinByMarketItem] ICollection<UserItemNode> userItems) {
            int num = 0;

            foreach (UserItemNode userItem in userItems) {
                if (userItem.Entity.IsSameGroup<UserGroupComponent>(SelfUserComponent.SelfUser) && !userItem.Entity.HasComponent<CreatedByRankItemComponent>()) {
                    num++;
                }
            }

            GarageItemsRegistry.GetItem<GarageItem>(marketItem.Entity).AdditionalPrice = CalculateAdditionalPrice(marketItem.itemAutoIncreasePrice, num);
        }

        [OnEventFire]
        public void FilterOwnItems(NewsItemFilterEvent e, NewsItemWithMarketItemGroupNode newsItem, [JoinByMarketItem] [Combine] SingleNode<UserItemComponent> userItem) {
            e.Hide = !userItem.Entity.HasComponent<UserItemCounterComponent>();
        }

        [OnEventFire]
        public void FilterFirstPurchase(NewsItemFilterEvent e, SingleNode<NewsItemComponent> newsItem, [JoinAll] Optional<ActivePaymentSaleNode> saleState) {
            if (IsFirstPurchaseNews(newsItem)) {
                e.Hide = !saleState.IsPresent() || !saleState.Get().activePaymentSale.Personal;
            }
        }

        bool IsFirstPurchaseNews(SingleNode<NewsItemComponent> newsItem) {
            if (string.IsNullOrEmpty(newsItem.component.Data.PreviewImageUrl)) {
                return false;
            }

            return newsItem.component.Data.PreviewImageUrl.Contains("illustration_sale");
        }

        [OnEventFire]
        public void AddItemPreview(NodeAddedEvent e, NewsItemUIWithMarketItemGroupNode newsItem, [JoinByMarketItem] MarketItemNode marketItem) {
            NewsItem data = newsItem.newsItem.Data;

            if (string.IsNullOrEmpty(data.PreviewImageUrl) && string.IsNullOrEmpty(data.PreviewImageGuid)) {
                string itemOrChildImage = GetItemOrChildImage(marketItem.Entity);

                if (itemOrChildImage != null) {
                    float aspectRatio = 1.734104f;
                    newsItem.newsItemUI.ImageContainer.SetImageSkin(itemOrChildImage, aspectRatio);
                    newsItem.newsItemUI.ImageContainer.FitInParent = true;
                }
            }
        }

        string GetItemOrChildImage(Entity item) {
            string text = null;

            if (item.HasComponent<ImageItemComponent>()) {
                text = item.GetComponent<ImageItemComponent>().SpriteUid;
            }

            if (text == null) {
                DefaultSkinWithImageNode defaultSkinWithImageNode = Select<DefaultSkinWithImageNode>(item, typeof(ParentGroupComponent)).FirstOrDefault();

                if (defaultSkinWithImageNode != null) {
                    text = defaultSkinWithImageNode.imageItem.SpriteUid;
                }
            }

            return text;
        }

        [OnEventFire]
        public void ApplySale(ApplyMarketItemSaleClientEvent e, MarketItemNode marketItem) {
            MarketItemSaleComponent marketItemSaleComponent = new();
            marketItemSaleComponent.endDate = e.EndDate;
            marketItem.Entity.RemoveComponentIfPresent<MarketItemSaleComponent>();
            marketItem.Entity.AddComponent(marketItemSaleComponent);
        }

        [OnEventFire]
        public void CancelSale(CancelMarketItemSaleClientEvent e, MarketItemNode marketItem) {
            marketItem.Entity.RemoveComponentIfPresent<MarketItemSaleComponent>();
        }

        int CalculateAdditionalPrice(ItemAutoIncreasePriceComponent increasePrice, int itemCount) {
            itemCount++;

            if (itemCount <= increasePrice.StartCount) {
                return 0;
            }

            long num = itemCount - increasePrice.StartCount;
            int num2 = (int)num * increasePrice.PriceIncreaseAmount;
            int maxAdditionalPrice = increasePrice.MaxAdditionalPrice;

            if (maxAdditionalPrice <= 0 || num2 < maxAdditionalPrice) {
                return num2;
            }

            return maxAdditionalPrice;
        }

        public class NewsItemWithMarketItemGroupNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public NewsItemComponent newsItem;
        }

        public class NewsItemUIWithMarketItemGroupNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public NewsItemComponent newsItem;

            public NewsItemUIComponent newsItemUI;
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class DefaultSkinWithImageNode : Node {
            public DefaultSkinItemComponent defaultSkinItem;
            public ImageItemComponent imageItem;

            public SkinItemComponent skinItem;
        }

        public class MarketItemWithFirstBuySaleNode : Node {
            public FirstBuySaleComponent firstBuySale;
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class MarketItemWithAutoIncreasePriceNode : Node {
            public ItemAutoIncreasePriceComponent itemAutoIncreasePrice;
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        [Not(typeof(CreatedByRankItemComponent))]
        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public UserGroupComponent userGroup;
            public UserItemComponent userItem;
        }

        public class UpdateGaragePriceEvent : Event { }

        public class ActivePaymentSaleNode : Node {
            public ActivePaymentSaleComponent activePaymentSale;

            public SelfUserComponent selfUser;
        }
    }
}