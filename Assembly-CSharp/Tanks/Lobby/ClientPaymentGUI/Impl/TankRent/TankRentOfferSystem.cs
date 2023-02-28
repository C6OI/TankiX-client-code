using System.Collections.Generic;
using System.Linq;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent {
    public class TankRentOfferSystem : ECSSystem {
        [OnEventFire]
        public void ShowLegendaryTankSpecialOfferStarted(NodeAddedEvent e, [Combine] TankRentOfferNode offer,
            [Context] [JoinBy(typeof(SpecialOfferGroupComponent))]
            OfferThatMustBeShown personalOffer, [Context] SingleNode<TankRentMainScreenElementsComponents> helper) {
            helper.component.tankRentOffer.SetActive(true);
            helper.component.SetButtonToOfferDisplayState();
            helper.component.tankRentButton.gameObject.SetActive(true);
            NewEvent<SpecialOfferScreenShownEvent>().Attach(personalOffer).Schedule();
        }

        [OnEventFire]
        public void ShowTanksScreen(ButtonClickEvent e, SingleNode<TankRentResearchConfirmButton> button, [JoinAll] SelfUserNode selfUser, [JoinAll] ICollection<TankRentOfferNode> offers,
            [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            helper.component.SetButtonToScreenDisplayState();
            helper.component.tankRentOffer.SetActive(false);
            helper.component.ShowTankRentScreen();

            foreach (TankRentOfferNode offer in offers) {
                IList<PersonalOfferNode> nodes = Select<PersonalOfferNode, SpecialOfferGroupComponent>(offer.Entity);

                NewEvent(new StartSpecialOfferEvent()).Attach(selfUser).Attach(nodes).Attach(offer)
                    .Schedule();
            }
        }

        [OnEventFire]
        public void SetButtonBehaviour(NodeAddedEvent e, [Combine] PersonalOfferNode personalOffer,
            [JoinBy(typeof(SpecialOfferGroupComponent))] [Context] [Combine]
            TankRentOfferNode offer, [Context] SelfUserNode selfUser, [Context] [Combine] OwnedPresetNode preset,
            [Context] SingleNode<TankRentMainScreenElementsComponents> helper) {
            if (preset.userGroup.Key != selfUser.userGroup.Key) {
                helper.component.SetButtonToScreenDisplayState();
            }
        }

        [OnEventFire]
        public void ShowRentButton(NodeAddedEvent e, [Combine] PersonalOfferNode offer, [JoinBy(typeof(SpecialOfferGroupComponent))] [Context] [Combine] TankRentOfferNode offer2,
            SingleNode<TankRentMainScreenElementsComponents> helper, [JoinAll] ICollection<TankRentOfferNode> specialOffers, [JoinAll] ICollection<PersonalOfferNode> personalOffers) {
            int num = personalOffers.Sum(personalOffer => specialOffers.Count(specialOffer => personalOffer.specialOfferGroup.Key == specialOffer.specialOfferGroup.Key));

            if (num == 3) {
                helper.component.tankRentButton.gameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void StartOffer(ButtonClickEvent e, SingleNode<TankRentLeafletComponent> leaflet, [JoinAll] SelfUserNode selfUser, [JoinByUser] [Combine] PersonalOfferNode personalOffer,
            [JoinBy(typeof(SpecialOfferGroupComponent))]
            TankRentOfferNode offer, [JoinBy(typeof(SpecialOfferGroupComponent))] OwnedPresetNode preset,
            [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            if (leaflet.component.TankRole == offer.legendaryTankSpecialOffer.TankRole) {
                helper.component.HideTankRentScreen();
                NewEvent(new MountPresetEvent()).Attach(preset).Schedule();
            }
        }

        [OnEventFire]
        public void BuyPreset(ConfirmButtonClickEvent e, SingleNode<TankPurchaseScreenComponent> purchaseScreen, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset,
            [JoinBy(typeof(SpecialOfferGroupComponent))]
            TankRentOfferNode offer, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper,
            [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            ShopDialogs dialogs2 = dialogs.component.Get<ShopDialogs>();
            purchaseScreen.component.OpenPurchaseWindow(offer.Entity, dialogs2);
        }

        [OnEventFire]
        public void SetTimer(NodeAddedEvent e, [Combine] SingleNode<TankRentLeafletComponent> leaflet, [Context] [Combine] PersonalOfferWithEndTimeNode personalOffer,
            [JoinBy(typeof(SpecialOfferGroupComponent))] [Context] [Combine]
            TankRentOfferNode offer, [Context] SingleNode<TankRentMainScreenElementsComponents> helper) {
            leaflet.component.starterPackTimer.onTimerExpired = delegate {
                helper.component.HideTankRentScreen();
            };

            float remaining = personalOffer.specialOfferEndTime.EndDate - Date.Now;
            leaflet.component.starterPackTimer.RunTimer(remaining);
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<TankPurchaseScreenComponent> starterPack) {
            starterPack.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<TankPurchaseScreenComponent> starterPack) {
            starterPack.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void OnDiscountAdded(NodeAddedEvent e, [Combine] TankRentOfferNode good, [Context] [JoinBy(typeof(SpecialOfferGroupComponent))] PersonalOfferNode personalOffer) {
            if (good.Entity.HasComponent<CustomOfferPriceForUIComponent>()) {
                good.Entity.RemoveComponent<CustomOfferPriceForUIComponent>();
            }

            double value = good.goodsPrice.Price * (1f - personalOffer.discount.DiscountCoeff);
            value = good.goodsPrice.Round(value);
            good.Entity.AddComponent(new CustomOfferPriceForUIComponent(value));
        }

        [OnEventFire]
        public void FillPurchaseWindow(NodeAddedEvent e, SingleNode<TankPurchaseScreenComponent> screen, [Context] SingleNode<SelectedPresetComponent> selectedPreset,
            [JoinBy(typeof(SpecialOfferGroupComponent))] [Context] [Combine]
            TankRentOfferNode offer,
            [JoinBy(typeof(SpecialOfferGroupComponent))] [Context]
            PersonalOfferNode personalOffer, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            ShopDialogs shopDialogs = dialogs.component.Get<ShopDialogs>();
            screen.component.InitiateScreen(offer.goodsPrice, personalOffer.discount, offer.legendaryTankSpecialOffer.TankRole, shopDialogs);
        }

        [OnEventFire]
        public void SetTimerOnPresetsScreen(NodeAddedEvent e, SingleNode<LegendaryTankTimerComponent> timer, [Context] SingleNode<SelectedPresetComponent> selectedPreset,
            [JoinBy(typeof(SpecialOfferGroupComponent))] [Context]
            PersonalOfferWithEndTimeNode personalOffer) {
            float remaining = personalOffer.specialOfferEndTime.EndDate - Date.Now;
            timer.component.timer.RunTimer(remaining);
        }

        [OnEventFire]
        public void HideMastery(NodeAddedEvent e, SingleNode<SelectedPresetComponent> selectedPreset, [Context] SingleNode<SelectedTurretUIComponent> selectedTurret,
            [Context] SingleNode<SelectedHullUIComponent> selectedHull) {
            if (selectedPreset.Entity.HasComponent<SpecialOfferGroupComponent>()) {
                selectedTurret.component.DisableMasteryElement();
                selectedHull.component.DisableMasteryElement();
            } else {
                selectedTurret.component.EnableMasteryElement();
                selectedHull.component.EnableMasteryElement();
            }
        }

        [OnEventFire]
        public void DisablePresetNameChange(NodeAddedEvent e, SingleNode<SelectedPresetComponent> selectedPreset, [Context] SingleNode<PresetNameEditorComponent> presetNameEditor) {
            if (selectedPreset.Entity.HasComponent<SpecialOfferGroupComponent>()) {
                presetNameEditor.component.DisableInput();
            } else {
                presetNameEditor.component.EnableInput();
            }
        }

        [OnEventFire]
        public void ScheduleButtonHide(NodeAddedEvent e, PersonalOfferWithEndTimeNode personalOffer,
            [Context] [JoinBy(typeof(SpecialOfferGroupComponent))] [Combine]
            TankRentOfferNode offer) {
            float timeInSec = personalOffer.specialOfferEndTime.EndDate - Date.Now;
            NewEvent(new HideOfferButtonEvent()).Attach(personalOffer).ScheduleDelayed(timeInSec);
        }

        [OnEventFire]
        public void HideOfferButton(HideOfferButtonEvent e, PersonalOfferWithEndTimeNode personalOffer, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void DisplayNumberOfBattlesLeft(NodeAddedEvent e, SingleNode<NumberOfBattlesLeftUIComponent> UI, SingleNode<SelectedPresetComponent> selectedPreset,
            [JoinBy(typeof(SpecialOfferGroupComponent))] [Context]
            SingleNode<NumberOfBattlesPlayedWithTankComponent> personalOffer) {
            UI.component.DisplayBattlesLeft(personalOffer.component.BattlesLeft);
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals,
            [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            deals.component.HandleSuccessMobile(e.TransactionId);
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals,
            [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals,
            [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals,
            [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            deals.component.HandleSuccess();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            deals.component.HandleGoToLink();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void GoToUrl(SteamBuyGoodsEvent e, Node any, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper) {
            deals.component.HandleGoToLink();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        public class TankRentOfferNode : Node {
            public GoodsPriceComponent goodsPrice;

            public LegendaryTankSpecialOfferComponent legendaryTankSpecialOffer;
            public SpecialOfferComponent specialOffer;

            public SpecialOfferGroupComponent specialOfferGroup;
        }

        public class PersonalOfferNode : Node {
            public DiscountComponent discount;
            public PersonalSpecialOfferPropertiesComponent personalSpecialOfferProperties;

            public SpecialOfferGroupComponent specialOfferGroup;
        }

        public class PersonalOfferWithEndTimeNode : PersonalOfferNode {
            public SpecialOfferEndTimeComponent specialOfferEndTime;
        }

        public class OfferThatMustBeShown : PersonalOfferNode {
            public SpecialOfferShowScreenForcedComponent specialOfferShowScreenForced;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class OwnedPresetNode : Node {
            public PresetItemComponent presetItem;

            public UserGroupComponent userGroup;
        }

        public class SelectedPresetNode : OwnedPresetNode {
            public SelectedPresetComponent selectedPreset;
        }

        public class HideOfferButtonEvent : Event { }
    }
}