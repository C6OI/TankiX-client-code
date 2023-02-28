using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class StarterPackSystem : ECSSystem {
        [OnEventFire]
        public void RemoveButtonOnComplete(NodeRemoveEvent e, [Combine] PersonalOfferNode personalOffer, [JoinAll] [Context] SingleNode<MainScreenComponent> mainScreen) {
            GameObject starterPackButton = mainScreen.component.StarterPackButton;

            if (starterPackButton != null) {
                starterPackButton.gameObject.SetActive(false);
            }
        }

        [OnEventFire]
        public void CloseScreenOnComplete(NodeRemoveEvent e, PersonalOfferNode personalOffer, [JoinAll] SingleNode<StarterPackScreenUIComponent> screen) {
            screen.component.Close();
        }

        [OnEventFire]
        public void CancelShopDialog(NodeRemoveEvent e, SingleNode<StarterPackScreenUIComponent> screen, [JoinAll] SingleNode<SelfUserComponent> user) {
            screen.component.shopDialogs.Cancel();
        }

        [OnEventFire]
        public void AddSpecialOfferButtonOnLobbyEnter(NodeAddedEvent e, SingleNode<MainScreenComponent> mainScreen, [Combine] PersonalOfferNode personalOffer,
            [Context] [JoinBy(typeof(SpecialOfferGroupComponent))]
            SpecialOfferNode specialOffer) {
            GameObject starterPackButton = mainScreen.component.StarterPackButton;
            StarterPackButtonComponent component = starterPackButton.GetComponent<StarterPackButtonComponent>();
            component.PackEntity = specialOffer.Entity;
            component.SetImage(specialOffer.starterPackVisualConfig.ButtonSpriteUid);
            starterPackButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowScreenAfterBattle(NodeAddedEvent e, [Combine] SpecialOfferNode specialOffer,
            [Context] [JoinBy(typeof(SpecialOfferGroupComponent))]
            PersonalOfferWithScreenNode personalOffer, [Context] [JoinAll] SingleNode<MainScreenComponent> mainScreen) {
            NewEvent<ShowStarterPackScreen>().Attach(specialOffer).ScheduleDelayed(0f);
            NewEvent<SpecialOfferScreenShownEvent>().Attach(personalOffer).Schedule();
        }

        [OnEventFire]
        public void OpenSpecialOfferScreen(ShowStarterPackScreen e, SpecialOfferNode specialOffer, [JoinAll] SingleNode<MainScreenComponent> mainScreen,
            [JoinAll] SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.CloseAll(string.Empty);
            GameObject starterPackScreen = mainScreen.component.StarterPackScreen;
            starterPackScreen.GetComponent<StarterPackScreenUIComponent>().shopDialogs = dialogs.component.Get<ShopDialogs>();
            starterPackScreen.GetComponent<StarterPackScreenUIComponent>().PackEntity = specialOffer.Entity;
            MainScreenComponent.Instance.ShowStarterPack();
        }

        [OnEventFire]
        public void CheckItem(RequestInfoForItemsEvent e, SingleNode<StarterPackMainElementComponent> starterPack, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystal,
            [JoinAll] XCrystalMarketItemNode xCrystal) {
            e.itemIds.ForEach(delegate(long id) {
                Entity entityById2 = GetEntityById(id);
                e.titles.Add(id, MarketItemNameLocalization.GetDetailedName(entityById2));
                e.previews.Add(id, entityById2.GetComponent<ImageItemComponent>().SpriteUid);
                ItemRarityComponent component = entityById2.GetComponent<ItemRarityComponent>();
                e.rarityFrames.Add(id, component.NeedRarityFrame);
                e.rarities.Add(id, component.RarityType);

                if (user.purchasedItemList.Contains(id)) {
                    e.purchased.Add(id);
                }
            });

            e.crystalTitle = crystal.descriptionItem.Name;
            e.crystalSprite = crystal.imageItem.SpriteUid;
            e.xCrystalTitle = xCrystal.descriptionItem.Name;
            e.xCrystalSprite = xCrystal.imageItem.SpriteUid;
            long itemId = starterPack.component.ItemId;
            Entity entityById = GetEntityById(itemId);
            e.mainItemId = itemId;
            e.mainItemDescription = starterPack.component.Description;
            e.mainItemSprite = entityById.GetComponent<ImageItemComponent>().SpriteUid;
            e.mainItemTitle = starterPack.component.Title;
            e.mainItemCount = starterPack.component.Count;
            e.mainItemCrystal = entityById == crystal.Entity;
            e.mainItemXCrystal = entityById == xCrystal.Entity;
        }

        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<StarterPackScreenUIComponent> starterPack) {
            starterPack.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<StarterPackScreenUIComponent> starterPack) {
            starterPack.component.Clear();
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<StarterPackScreenUIComponent> starterPack) {
            starterPack.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals) {
            Log.Debug("SuccessMobile");
            deals.component.HandleSuccessMobile(e.TransactionId);
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals) {
            Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals) {
            Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals) {
            Log.Debug("Success");
            deals.component.HandleSuccess();
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals) {
            Log.Debug("GoToUrl");
            deals.component.HandleGoToLink();
        }

        public class PersonalOfferNode : Node {
            public PersonalSpecialOfferScreenComponent personalSpecialOfferScreen;

            public SpecialOfferRemainingTimeComponent specialOfferRemainingTime;
            public SpecialOfferVisibleComponent specialOfferVisible;
        }

        public class PersonalOfferWithScreenNode : PersonalOfferNode {
            public SpecialOfferShowScreenForcedComponent specialOfferShowScreenForced;
        }

        public class SpecialOfferNode : Node {
            public BaseStarterPackSpecialOfferComponent baseStarterPackSpecialOffer;

            public CountableItemsPackComponent countableItemsPack;

            public CrystalsPackComponent crystalsPack;

            public GoodsPriceComponent goodsPrice;

            public ItemsPackFromConfigComponent itemsPackFromConfig;
            public SpecialOfferComponent specialOffer;

            public SpecialOfferContentComponent specialOfferContent;

            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;

            public SpecialOfferEndTimeComponent specialOfferEndTime;

            public SpecialOfferGroupComponent specialOfferGroup;

            public SpecialOfferScreenLocalizationComponent specialOfferScreenLocalization;

            public StarterPackVisualConfigComponent starterPackVisualConfig;

            public XCrystalsPackComponent xCrystalsPack;
        }

        public class StarterPackRegistrationNode : Node {
            public CountableItemsPackComponent countableItemsPack;

            public CrystalsPackComponent crystalsPack;
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SelfUserNode : Node {
            public PurchasedItemListComponent purchasedItemList;
            public SelfUserComponent selfUser;
        }

        public class MarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ImageItemComponent imageItem;

            public MarketItemComponent marketItem;
        }

        public class CrystalMarketItemNode : MarketItemNode {
            public CrystalItemComponent crystalItem;
        }

        public class XCrystalMarketItemNode : MarketItemNode {
            public XCrystalItemComponent xCrystalItem;
        }

        public class ShowStarterPackScreen : Event { }
    }
}