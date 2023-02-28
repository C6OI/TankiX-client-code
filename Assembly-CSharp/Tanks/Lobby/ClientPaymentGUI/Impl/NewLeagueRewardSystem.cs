using System.Collections.Generic;
using System.Linq;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class NewLeagueRewardSystem : ECSSystem {
        [OnEventFire]
        public void ShowLeagueReward(ShowRewardEvent e, ScreenNode screen, LeaguePersonalRewardNode reward, [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods) {
            LeagueFirstEntrancePersonalBattleRewardComponent leagueFirstEntrancePersonalBattleReward = reward.leagueFirstEntrancePersonalBattleReward;
            Entity personalOffer = leagueFirstEntrancePersonalBattleReward.PersonalOffer;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(personalOffer.GetComponent<SpecialOfferGroupComponent>().Key);
            SpecialOfferContentLocalizationComponent component = entity.GetComponent<SpecialOfferContentLocalizationComponent>();
            string title = component.Title;
            string description = component.Description;
            List<SpecialOfferItem> list = new();
            CountableItemsPackComponent component2 = entity.GetComponent<CountableItemsPackComponent>();

            foreach (KeyValuePair<long, int> item in component2.Pack) {
                long key = item.Key;
                Entity entity2 = Flow.Current.EntityRegistry.GetEntity(key);
                int value = item.Value;
                string spriteUid = entity2.GetComponent<ImageItemComponent>().SpriteUid;
                string name = entity2.GetComponent<DescriptionItemComponent>().Name;
                list.Add(new SpecialOfferItem(value, spriteUid, name));
            }

            double price = entity.GetComponent<GoodsPriceComponent>().Price;
            string currency = entity.GetComponent<GoodsPriceComponent>().Currency;
            int discount = 0;
            int labelPercentage = 0;

            if (entity.HasComponent<LeagueFirstEntranceSpecialOfferComponent>()) {
                labelPercentage = entity.GetComponent<LeagueFirstEntranceSpecialOfferComponent>().WorthItPercent;
            }

            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(title, description, list);
            specialOfferUI.SetPriceButton(discount, price, labelPercentage, currency);
            specialOfferUI.Appear();
            specialOfferUI.gameObject.AddComponent<NewLeaguePurchaseItemComponent>();

            foreach (SingleNode<PaymentMethodComponent> method in methods) {
                specialOfferUI.gameObject.GetComponent<NewLeaguePurchaseItemComponent>().AddMethod(method.Entity);
            }
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent e, ScreenNode screen) {
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            NewLeaguePurchaseItemComponent component = specialOfferUI.gameObject.GetComponent<NewLeaguePurchaseItemComponent>();

            if (component != null) {
                Object.Destroy(component);
            }
        }

        [OnEventFire]
        public void OnPriceButtonClick(ButtonClickEvent e, SingleNode<SpecialOfferPriceButtonComponent> button, [JoinAll] [Mandatory] ScreenNode screen,
            [JoinBy(typeof(BattleRewardGroupComponent))]
            SingleNode<LeagueFirstEntrancePersonalBattleRewardComponent> reward,
            [JoinAll] [Mandatory] SingleNode<Dialogs60Component> dialogs) {
            Entity personalOffer = reward.component.PersonalOffer;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(personalOffer.GetComponent<SpecialOfferGroupComponent>().Key);
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            ShopDialogs shopDialogs = dialogs.component.Get<ShopDialogs>();
            specialOfferUI.GetComponent<NewLeaguePurchaseItemComponent>().ShowPurchaseDialog(shopDialogs, entity);
        }

        [OnEventFire]
        public void ChangeButtonOnBuy(NodeAddedEvent e, SingleNode<SpecialOfferPaidComponent> personalOffer,
            [JoinBy(typeof(SpecialOfferGroupComponent))]
            SingleNode<LeagueFirstEntranceSpecialOfferComponent> specialOffer, [JoinAll] ScreenNode screen,
            [JoinBy(typeof(BattleRewardGroupComponent))]
            SingleNode<LeagueFirstEntrancePersonalBattleRewardComponent> reward) {
            BattleResultSpecialOfferUiComponent ui = screen.battleResultsAwardsScreen.specialOfferUI;
            KeyValuePair<long, int> keyValuePair = specialOffer.Entity.GetComponent<CountableItemsPackComponent>().Pack.FirstOrDefault();
            long key = keyValuePair.Key;
            int value = keyValuePair.Value;

            if (key == 0 || !Flow.Current.EntityRegistry.ContainsEntity(key)) {
                return;
            }

            Entity entity = Flow.Current.EntityRegistry.GetEntity(key);

            if (entity.HasComponent<ContainerMarkerComponent>()) {
                ui.SetOpenButton(key, value, delegate {
                    ui.DeactivateAllButtons();
                });
            }
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<NewLeaguePurchaseItemComponent> starterPack) {
            starterPack.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals) {
            deals.component.HandleSuccessMobile(e.TransactionId);
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals) {
            Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals) {
            Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals) {
            deals.component.HandleSuccess();
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] ScreenNode screen) {
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            NewLeaguePurchaseItemComponent component = specialOfferUI.GetComponent<NewLeaguePurchaseItemComponent>();

            if (component != null) {
                component.HandleGoToLink();
            }
        }

        public class PersonalRewardNode : Node {
            public BattleRewardGroupComponent battleRewardGroup;
            public PersonalBattleRewardComponent personalBattleReward;
        }

        public class LeaguePersonalRewardNode : PersonalRewardNode {
            public LeagueFirstEntrancePersonalBattleRewardComponent leagueFirstEntrancePersonalBattleReward;
        }

        public class BattleResultsNode : Node {
            public BattleResultsComponent battleResults;
        }

        public class ScreenNode : Node {
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
        }
    }
}