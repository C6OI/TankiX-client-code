using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using TMPro;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;
using Object = UnityEngine.Object;
using OrderItemComponent = Tanks.Lobby.ClientPayment.Impl.OrderItemComponent;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumUiShopSystem : ECSSystem {
        [OnEventFire]
        public void CreatePacks(NodeAddedEvent e, SingleNode<PremiumShopTabComponent> shopNode, [JoinAll] ICollection<PremiumGoodsNode> goods, [JoinAll] BaseUserNode userNode) {
            List<PremiumGoodsNode> list = BuildList(goods, userNode);
            list.Sort(new PremiumGoodsNodeComparer());

            for (int i = 0; i < list.Count; i++) {
                PremiumPackComponent component = Object.Instantiate(shopNode.component.PackPrefab, shopNode.component.PackContainer).GetComponent<PremiumPackComponent>();
                FillPack(component, list[i], i);
            }
        }

        [OnEventFire]
        public void DestroyPacks(NodeRemoveEvent e, SingleNode<PremiumShopTabComponent> shopNode) {
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

        List<PremiumGoodsNode> BuildList(ICollection<PremiumGoodsNode> goods, BaseUserNode userNode) {
            List<PremiumGoodsNode> list = new();
            int rank = userNode.userRank.Rank;

            foreach (PremiumGoodsNode good in goods) {
                int minRank = good.premiumOffer.MinRank;
                int maxRank = good.premiumOffer.MaxRank;

                if (rank >= minRank && rank < maxRank) {
                    list.Add(good);
                }
            }

            return list;
        }

        void FillPack(PremiumPackComponent pack, PremiumGoodsNode packNode, int count) {
            GetDiscountForOfferEvent getDiscountForOfferEvent = new();
            ScheduleEvent(getDiscountForOfferEvent, packNode);
            pack.DaysText = packNode.countableItemsPack.Pack.First().Value.ToString();
            pack.DaysDescription = packNode.specialOfferContentLocalization.Description;
            pack.Price = string.Format("{0:0.00} {1}", (1f - getDiscountForOfferEvent.Discount) * packNode.goodsPrice.Price, packNode.goodsPrice.Currency);
            pack.Discount = getDiscountForOfferEvent.Discount;
            pack.HasXCrystals = IsGoodsWithCrystals(packNode);
            pack.LearnMoreIndex = count;
            pack.GoodsEntity = packNode.Entity;
        }

        bool IsGoodsWithCrystals(PremiumGoodsNode sortedGood) => sortedGood.countableItemsPack.Pack.ContainsKey(-180272377L);

        [OnEventFire]
        public void OnDiscountAdded(NodeAddedEvent e, PremiumGoodsNode good, [Context] [JoinBy(typeof(SpecialOfferGroupComponent))] SingleNode<DiscountComponent> personalOffer) {
            if (good.Entity.HasComponent<CustomOfferPriceForUIComponent>()) {
                good.Entity.RemoveComponent<CustomOfferPriceForUIComponent>();
            }

            double value = good.goodsPrice.Price * (1f - personalOffer.component.DiscountCoeff);
            value = good.goodsPrice.Round(value);
            good.Entity.AddComponent(new CustomOfferPriceForUIComponent(value));
        }

        [OnEventFire]
        public void SetDiscount(GetDiscountForOfferEvent e, PremiumGoodsNode good, [JoinBy(typeof(SpecialOfferGroupComponent))] Optional<SingleNode<DiscountComponent>> personalOffer) {
            e.Discount = 0f;

            if (personalOffer.IsPresent()) {
                e.Discount = personalOffer.Get().component.DiscountCoeff;
            }
        }

        [OnEventFire]
        public void ShowPremiumActivatedDialog(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, PremiumBoostItemDurationChangedNode boostItem,
            [JoinByUser] SelfPremiumUserNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            NewEvent<ShowPremiumActivatedDialogEvent>().AttachAll(user.Entity, boostItem.Entity).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowPremiumActivatedDialog(ShowPremiumActivatedDialogEvent e, SelfPremiumUserNode user, PremiumBoostItemDurationChangedNode boostItem,
            [JoinByUser] Optional<PremiumQuestItemDurationChangedNode> questItem, [JoinAll] SingleNode<MainScreenComponent> homeScreen,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            Action action = null;
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            PremiumActivatedUIComponent premiumActivatedUiComponent = dialogs.component.Get<PremiumActivatedUIComponent>();
            int days = Convert.ToInt32(user.premiumAccountBoost.EndDate.UnityTime / 86400f);

            if (boostItem.Entity.HasComponent<PremiumPromoComponent>()) {
                if (questItem.IsPresent()) {
                    action = delegate {
                        premiumActivatedUiComponent.ShowPrem(animators, true, days, true);
                    };

                    boostItem.Entity.RemoveComponent<PremiumPromoComponent>();
                    questItem.Get().Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    return;
                }

                action = delegate {
                    premiumActivatedUiComponent.ShowPrem(animators, false, days, true);
                };

                boostItem.Entity.RemoveComponent<PremiumPromoComponent>();
                boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
            } else {
                if (questItem.IsPresent()) {
                    action = delegate {
                        premiumActivatedUiComponent.ShowPrem(animators, true, days);
                    };

                    questItem.Get().Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    return;
                }

                action = delegate {
                    premiumActivatedUiComponent.ShowPrem(animators, false, days);
                };

                boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
            }

            SingleNode<ActiveNotificationComponent> singleNode = SelectAll<SingleNode<ActiveNotificationComponent>>().FirstOrDefault();

            if (singleNode == null && action != null) {
                action();
            }
        }

        [OnEventFire]
        public void HideActivateNotification(NodeAddedEvent e, SingleNode<ActiveNotificationComponent> notif, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.Get<PremiumActivatedUIComponent>().HideImmediate();
        }

        [OnEventFire]
        public void ActivatePremiumMainScreenActiveIcon(NodeAddedEvent e, SelfPremiumUserNode user, SingleNode<PremiumMainScreenButtonComponent> button) {
            button.component.ActivatePremium();
        }

        [OnEventFire]
        public void DeactivatePremiumMainScreenActiveIcon(NodeRemoveEvent e, SelfPremiumUserNode user, SingleNode<PremiumMainScreenButtonComponent> button) {
            button.component.DeactivatePremium();
        }

        [OnEventFire]
        public void DiscountActivated(NodeAddedEvent e, SingleNode<PremiumAccountDiscountActivatedComponent> user, SingleNode<MainScreenComponent> homeScreen,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            PremiumExpiredUiComponent premiumExpiredUiComponent = dialogs.component.Get<PremiumExpiredUiComponent>();
            premiumExpiredUiComponent.Show(animators);
            user.Entity.RemoveComponent<PremiumAccountDiscountActivatedComponent>();
        }

        [OnEventFire]
        public void ActivatePremiumQuestLine(NodeAddedEvent e, SelfPremiumQuestUserNode user, SingleNode<PremiumToolbarUiComponent> toolbar) {
            toolbar.component.ActivatePremiumTasks();
        }

        [OnEventFire]
        public void DeactivatePremiumQuestLine(NodeRemoveEvent e, SelfPremiumQuestUserNode user, SingleNode<PremiumToolbarUiComponent> toolbar) {
            toolbar.component.DeactivatePremiumTasks();
        }

        [OnEventFire]
        public void FillDaysLeft(UpdateEvent e, SelfPremiumUserNode user, [JoinAll] SingleNode<PremiumToolbarUiComponent> toolbar) {
            if (toolbar.component.visible) {
                TextMeshProUGUI activeText = toolbar.component.activeText;
                Date endDate = user.premiumAccountBoost.EndDate;
                float num = endDate.UnityTime / 86400f;
                float num2 = endDate.UnityTime / 3600f;

                string text = !(num > 1f) ? string.Format(toolbar.component.hoursTextLocalizedField.Value, num2.ToString("####"))
                                  : string.Format(toolbar.component.daysTextLocalizedField.Value, num.ToString("####"));

                activeText.text = text;
            }
        }

        [OnEventComplete]
        public void PremiumMainScreenButtonClick(ButtonClickEvent e, SingleNode<PremiumMainScreenButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] BaseUserNode selfUser, [JoinAll] SingleNode<PremiumToolbarUiComponent> premiumToolbar) {
            if (selfUser.Entity.HasComponent<PremiumAccountBoostComponent>()) {
                premiumToolbar.component.Toggle();
                dialogs.component.Get<PremiumLearnMoreComponent>().HideImmediate();
            }
        }

        public class BaseUserNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class SelfPremiumUserNode : BaseUserNode {
            public PremiumAccountBoostComponent premiumAccountBoost;
        }

        public class SelfPremiumQuestUserNode : BaseUserNode {
            public PremiumAccountQuestComponent premiumAccountQuest;
        }

        public class PremiumGoodsNode : Node {
            public CountableItemsPackComponent countableItemsPack;

            public GoodsPriceComponent goodsPrice;

            public OrderItemComponent orderItem;
            public PremiumOfferComponent premiumOffer;

            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;
        }

        public class PremiumBoostItemDurationChangedNode : Node {
            public DurationUserItemComponent durationUserItem;
            public PremiumBoostItemComponent premiumBoostItem;

            public PremiumDurationChangedComponent premiumDurationChanged;
        }

        public class PremiumQuestItemDurationChangedNode : Node {
            public DurationUserItemComponent durationUserItem;

            public PremiumDurationChangedComponent premiumDurationChanged;
            public PremiumQuestItemComponent premiumQuestItem;
        }

        public class GetDiscountForOfferEvent : Event {
            public float Discount { get; set; }
        }

        class PremiumGoodsNodeComparer : IComparer<PremiumGoodsNode> {
            public int Compare(PremiumGoodsNode a, PremiumGoodsNode b) => a.orderItem.Index.CompareTo(b.orderItem.Index);
        }

        public class ShowPremiumActivatedDialogEvent : Event { }
    }
}