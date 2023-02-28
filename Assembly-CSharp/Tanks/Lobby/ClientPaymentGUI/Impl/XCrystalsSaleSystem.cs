using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientLocale.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class XCrystalsSaleSystem : ECSSystem {
        [OnEventFire]
        public void OnAddPersonalXCrystalBonusUILabel(NodeAddedEvent e, SingleNode<PersonalXCrystalBonusUIComponent> personalXCrystalBonusUI, UserNode userNode) {
            GameObject gameObject = personalXCrystalBonusUI.component.gameObject;
            Entity entity = userNode.Entity;
            bool active = entity.HasComponent<ActivePaymentSaleComponent>() && entity.GetComponent<ActivePaymentSaleComponent>().PersonalXCrystalBonus;
            gameObject.SetActive(active);
        }

        [OnEventFire]
        public void OnAddPersonalXCrystalBonusUILabel(NodeAddedEvent e, SingleNode<PersonalXCrystalBonusUIComponent> personalXCrystalBonusUI, SaleNode sale) {
            personalXCrystalBonusUI.component.gameObject.SetActive(sale.activePaymentSale.PersonalXCrystalBonus);
        }

        [OnEventFire]
        public void OnRemovePersonalXCrystalBonusUILabel(NodeRemoveEvent e, SaleNode sale, [JoinAll] SingleNode<PersonalXCrystalBonusUIComponent> personalXCrystalBonusUI) {
            personalXCrystalBonusUI.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void OnAddPersonalDiscountUILabel(NodeAddedEvent e, SingleNode<PersonalDiscountUIComponent> personalDiscountUI, UserNode userNode) {
            GameObject gameObject = personalDiscountUI.component.gameObject;
            Entity entity = userNode.Entity;
            bool flag = entity.HasComponent<ActivePaymentSaleComponent>() && entity.GetComponent<ActivePaymentSaleComponent>().PersonalPriceDiscount;

            if (flag) {
                personalDiscountUI.component.EndDate = entity.GetComponent<ActivePaymentSaleComponent>().StopTime;
            }

            gameObject.SetActive(flag);
        }

        [OnEventFire]
        public void OnAddPersonalDiscountUILabel(NodeAddedEvent e, SingleNode<PersonalDiscountUIComponent> personalDiscountUI, SaleNode sale) {
            personalDiscountUI.component.EndDate = sale.activePaymentSale.StopTime;
            personalDiscountUI.component.gameObject.SetActive(sale.activePaymentSale.PersonalPriceDiscount);
        }

        [OnEventFire]
        public void OnRemovePersonalDiscountUILabel(NodeRemoveEvent e, SaleNode sale, [JoinAll] SingleNode<PersonalDiscountUIComponent> personalDiscountUI) {
            personalDiscountUI.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void OnAddXCrystalsSaleEndTimerLabel(NodeAddedEvent e, SingleNode<XCrystalsSaleEndTimerComponent> saleTimer, UserNode userNode,
            [JoinAll] Optional<SingleNode<GiftPromoUIDataComponent>> giftPromo) {
            if (giftPromo.IsPresent()) {
                saleTimer.component.EndDate = new Date(float.NegativeInfinity);
                return;
            }

            Entity entity = userNode.Entity;

            if (!entity.HasComponent<ActivePaymentSaleComponent>()) {
                saleTimer.component.EndDate = new Date(float.NegativeInfinity);
                return;
            }

            ActivePaymentSaleComponent component = entity.GetComponent<ActivePaymentSaleComponent>();
            saleTimer.component.EndDate = !component.Personal ? component.StopTime : new Date(float.NegativeInfinity);
        }

        [OnEventFire]
        public void OnAddXCrystalsSaleEndTimerLabel(NodeAddedEvent e, SingleNode<XCrystalsSaleEndTimerComponent> saleTimer, SaleNode sale,
            [JoinAll] Optional<SingleNode<GiftPromoUIDataComponent>> giftPromo) {
            if (giftPromo.IsPresent()) {
                saleTimer.component.EndDate = new Date(float.NegativeInfinity);
            } else {
                saleTimer.component.EndDate = !sale.activePaymentSale.Personal ? sale.activePaymentSale.StopTime : new Date(float.NegativeInfinity);
            }
        }

        [OnEventFire]
        public void OnRemoveXCrystalsSaleEndTimerLabel(NodeRemoveEvent e, SaleNode sale, [JoinAll] SingleNode<XCrystalsSaleEndTimerComponent> saleTimer) {
            saleTimer.component.EndDate = new Date(float.NegativeInfinity);
        }

        [OnEventFire]
        public void OnAddCustomTextUI(NodeAddedEvent e, SingleNode<CustomDiscountUIComponent> customDiscountUI,
            [JoinAll] Optional<SingleNode<CustomDiscountTextComponent>> customDiscountText) {
            if (!customDiscountText.IsPresent()) {
                customDiscountUI.component.description.text = string.Empty;
            }
        }

        [OnEventFire]
        public void OnAddCustomTextUI(NodeAddedEvent e, SaleNode sale, [JoinAll] Optional<SingleNode<CustomDiscountUIComponent>> customDiscountUI) {
            if (customDiscountUI.IsPresent() && sale.activePaymentSale.Personal) {
                customDiscountUI.Get().component.description.text = string.Empty;
            }
        }

        [OnEventFire]
        public void OnRemoveSale(NodeRemoveEvent e, SaleNode sale, [JoinAll] SingleNode<CustomDiscountTextComponent> customDiscountText,
            [JoinAll] SingleNode<CustomDiscountUIComponent> customDiscountUI, [JoinAll] Optional<SingleNode<SteamComponent>> steam) {
            string text = customDiscountText.component.Get(LocaleUtils.GetSavedLocaleCode(), steam.IsPresent());
            customDiscountUI.component.description.text = text;
        }

        [OnEventFire]
        public void OnAddCustomText(NodeAddedEvent e, SingleNode<CustomDiscountTextComponent> customDiscountText, SingleNode<CustomDiscountUIComponent> customDiscountUI,
            [JoinAll] Optional<SaleNode> sale, [JoinAll] Optional<SingleNode<SteamComponent>> steam) {
            if (sale.IsPresent() && sale.Get().activePaymentSale.Personal) {
                customDiscountUI.component.description.text = string.Empty;
                return;
            }

            string text = customDiscountText.component.Get(LocaleUtils.GetSavedLocaleCode(), steam.IsPresent());
            customDiscountUI.component.description.text = text;
        }

        [OnEventFire]
        public void OnRemoveText(NodeRemoveEvent e, SingleNode<CustomDiscountTextComponent> customDiscountText, [JoinAll] SingleNode<CustomDiscountUIComponent> customDiscountUI) {
            customDiscountUI.component.description.text = string.Empty;
        }

        [OnEventFire]
        public void OnAddLabel(NodeAddedEvent e, SingleNode<GiftPromoUIDataComponent> giftPromo, SingleNode<CustomDiscountUIComponent> customDiscountUI) {
            customDiscountUI.component.description.text = giftPromo.component.Get(LocaleUtils.GetSavedLocaleCode());
        }

        [OnEventFire]
        public void OnRemoveText(NodeRemoveEvent e, SingleNode<GiftPromoUIDataComponent> giftPromo, [JoinAll] SingleNode<CustomDiscountUIComponent> customDiscountUI) {
            customDiscountUI.component.description.text = string.Empty;
        }

        [OnEventFire]
        public void TurnOffSaleTimer(NodeAddedEvent e, SingleNode<XCrystalsSaleEndTimerComponent> saleEndTimer, SingleNode<GiftPromoUIDataComponent> giftPromo) {
            saleEndTimer.component.EndDate = new Date(float.NegativeInfinity);
            saleEndTimer.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void OnRemoveGift(NodeRemoveEvent e, SingleNode<GiftPromoUIDataComponent> giftPromo, [JoinAll] SingleNode<XCrystalsSaleEndTimerComponent> saleEndTimer, SaleNode sale) {
            saleEndTimer.component.EndDate = !sale.activePaymentSale.Personal ? sale.activePaymentSale.StopTime : new Date(float.NegativeInfinity);
        }

        [OnEventFire]
        public void Mark(NodeAddedEvent e, SaleNode saleState, [JoinAll] [Combine] SingleNode<GoodsComponent> goods) {
            goods.component.SaleState.PriceMultiplier = saleState.activePaymentSale.PriceMultiplier;
            goods.component.SaleState.AmountMultiplier = saleState.activePaymentSale.AmountMultiplier;
            ScheduleEvent<GoodsChangedEvent>(goods);
        }

        [OnEventFire]
        public void Mark(NodeRemoveEvent e, SaleNode saleState, [JoinAll] [Combine] SingleNode<GoodsComponent> goods) {
            goods.component.SaleState.Reset();
            ScheduleEvent<GoodsChangedEvent>(goods);
        }

        [OnEventFire]
        public void Mark(NodeAddedEvent e, [Combine] SingleNode<GoodsComponent> goods, Optional<SaleNode> saleState) {
            if (saleState.IsPresent()) {
                goods.component.SaleState.PriceMultiplier = saleState.Get().activePaymentSale.PriceMultiplier;
                goods.component.SaleState.AmountMultiplier = saleState.Get().activePaymentSale.AmountMultiplier;
            } else {
                goods.component.SaleState.Reset();
            }

            ScheduleEvent<GoodsChangedEvent>(goods);
        }

        [OnEventFire]
        public void SetSaleIcon(NodeAddedEvent e, SingleNode<ShopBadgeComponent> indicator, SaleNode sale) {
            indicator.component.SaleAvailable = !sale.activePaymentSale.Personal;
            indicator.component.PersonalDiscountAvailable = sale.activePaymentSale.Personal;
        }

        [OnEventFire]
        public void SetSaleIcon(NodeRemoveEvent e, SaleNode sale, [Combine] [JoinAll] SingleNode<ShopBadgeComponent> indicator) {
            indicator.component.SaleAvailable = false;
            indicator.component.PersonalDiscountAvailable = false;
        }

        [OnEventFire]
        public void SetSaleIcon(NodeRemoveEvent e, [Combine] SingleNode<ShopBadgeComponent> indicator) {
            indicator.component.SaleAvailable = false;
            indicator.component.PersonalDiscountAvailable = false;
        }

        [OnEventFire]
        public void SetPromoIcon(NodeAddedEvent e, SingleNode<GiftPromoUIDataComponent> promo, [Combine] SingleNode<ShopBadgeComponent> indicator) {
            indicator.component.SetPromoAvailable(promo.component.PromoKey, true);
        }

        [OnEventFire]
        public void SetPromoIcon(NodeRemoveEvent e, SingleNode<GiftPromoUIDataComponent> promo, [Combine] [JoinAll] SingleNode<ShopBadgeComponent> indicator) {
            indicator.component.SetPromoAvailable(promo.component.PromoKey, false);
        }

        [OnEventFire]
        public void SetPromoIcon(NodeRemoveEvent e, SingleNode<ShopBadgeComponent> indicator, [JoinAll] SingleNode<GiftPromoUIDataComponent> promo) {
            indicator.component.SetPromoAvailable(promo.component.PromoKey, false);
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class SaleNode : UserNode {
            public ActivePaymentSaleComponent activePaymentSale;
        }
    }
}