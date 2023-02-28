using System.Collections.Generic;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientProfile.API;
using tanks.modules.lobby.ClientPayment.Scripts.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ShopXCrystalsSystem : ECSSystem {
        [OnEventFire]
        public void OpenShopScreen(GoToXCryShopScreen e, Node any) {
            MainScreenComponent.Instance.ShowShopIfNotVisible();

            if (ShopTabManager.Instance == null) {
                ShopTabManager.shopTabIndex = 3;
            } else {
                ShopTabManager.Instance.Show(3);
            }
        }

        [OnEventFire]
        public void OpenSynthCryScreen(GoToExchangeCryScreen e, Node any) {
            MainScreenComponent.Instance.ShowShopIfNotVisible();

            if (ShopTabManager.Instance == null) {
                ShopTabManager.shopTabIndex = 4;
            } else {
                ShopTabManager.Instance.Show(4);
            }

            MainScreenComponent.Instance.GetComponentInChildren<SynthUIComponent>(true).SetCrystals(e.ExchangingCrystalls);
        }

        [OnEventFire]
        public void InitDialogs(NodeAddedEvent e, SingleNode<ShopXCrystalsComponent> shop, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            shop.component.shopDialogs = dialogs.component.Get<ShopDialogs>();
        }

        [OnEventFire]
        public void InitShop(NodeAddedEvent e, SingleNode<ShopComponent> shop, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<GoToPaymentRequestEvent>(user);
            ScheduleEvent<ResetPreviewEvent>(user);
        }

        [OnEventFire]
        public void CloseShop(NodeRemoveEvent e, SingleNode<ShopComponent> shop, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            ShopDialogs shopDialogs = dialogs.component.Get<ShopDialogs>();
            shopDialogs.Cancel();
        }

        [OnEventFire]
        public void ReloadPaymentSectionOnChangeCountry(ConfirmUserCountryEvent e, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);
        }

        [OnEventFire]
        public void OpenPaymentSection(NodeAddedEvent e, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);
        }

        [OnEventFire]
        public void SteamConponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> steamNode, [Context] SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<DealsUIComponent> deasUINode) {
            deasUINode.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void AddPack(NodeAddedEvent e, [Combine] XCrystalPack pack, SingleNode<ShopXCrystalsComponent> shop, SingleNode<PacksImagesComponent> images,
            [JoinAll] Optional<SingleNode<PaymentGiftsComponent>> gifts) {
            if (pack.Entity.HasComponent<PaymentGiftComponent>()) {
                pack.Entity.RemoveComponent<PaymentGiftComponent>();
            }

            if (gifts.IsPresent() && gifts.Get().component.Gifts.ContainsKey(pack.packId.Id)) {
                pack.Entity.AddComponent(new PaymentGiftComponent(gifts.Get().component.Gifts[pack.packId.Id]));
            }

            shop.component.AddPackage(pack.Entity, images.component.AmountToImages[pack.xCrystalsPack.Amount]);
            shop.component.Arange();
        }

        [OnEventComplete]
        public void AddGift(NodeAddedEvent e, SingleNode<PaymentGiftsComponent> gifts, ICollection<XCrystalPack> packs, SingleNode<ShopXCrystalsComponent> shop) {
            foreach (XCrystalPack pack in packs) {
                if (pack.Entity.HasComponent<PaymentGiftComponent>()) {
                    pack.Entity.RemoveComponent<PaymentGiftComponent>();
                }

                if (gifts.component.Gifts.ContainsKey(pack.packId.Id)) {
                    pack.Entity.AddComponent(new PaymentGiftComponent(gifts.component.Gifts[pack.packId.Id]));
                }

                shop.component.UpdatePackage(pack.Entity);
            }
        }

        [OnEventFire]
        public void RemoveGift(NodeRemoveEvent e, SingleNode<PaymentGiftsComponent> gifts, [JoinAll] ICollection<XCrystalPack> packs, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            foreach (XCrystalPack pack in packs) {
                if (pack.Entity.HasComponent<PaymentGiftComponent>()) {
                    pack.Entity.RemoveComponent<PaymentGiftComponent>();
                }

                shop.component.UpdatePackage(pack.Entity);
            }
        }

        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.HandleGoToLink();
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.HandleSuccessMobile(e.TransactionId);
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.HandleQiwiError();
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            Log.Error("Error making payment: " + e.ErrorCode);
            shop.component.HandleError();
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.HandleSuccess();
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.Clear();
        }

        [OnEventFire]
        public void SetCode(NodeAddedEvent e, SingleNode<PhoneInputComponent> input, SingleNode<PhoneCodesComponent> codes, SelfUserNode user) {
            input.component.SetCode(codes.component.Codes[user.userCountry.CountryCode]);
        }

        [OnEventFire]
        public void UpdatePack(GoodsChangedEvent e, PackNode pack, [JoinAll] SingleNode<ShopXCrystalsComponent> shop) {
            shop.component.UpdatePackage(pack.Entity);
        }

        [OnEventFire]
        public void CheckPayment(NodeAddedEvent e, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<CheckGiftsEvent>(user);
        }

        [OnEventFire]
        public void OpenGameCurrency(GoToPaymentEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);
        }

        [OnEventFire]
        public void OpenGameCurrency(GoToSteamPaymentEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);
        }

        [OnEventFire]
        public void GetCompensation(CalculateCompensationResponseEvent e, SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<WarningWindowComponent> window) {
            window.component.SetCompensation(e.Amount);
        }

        [OnEventFire]
        public void OnSynth(UserXCrystalsChangedEvent e, SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<SynthUIComponent> synth) {
            synth.component.OnXCrystalsChanged();
        }

        [OnEventFire]
        public void ResetPreview(NodeRemoveEvent e, SingleNode<ContainersUI> containersUi) {
            ScheduleEvent<ResetPreviewEvent>(containersUi);
        }

        public class PackNode : Node {
            public GoodsComponent goods;

            public GoodsPriceComponent goodsPrice;

            public PackIdComponent packId;
            public XCrystalsPackComponent xCrystalsPack;
        }

        [Not(typeof(SpecialOfferComponent))]
        public class XCrystalPack : PackNode { }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserCountryComponent userCountry;
        }
    }
}