using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientProfile.Impl;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentSectionSystem : ECSSystem {
        [OnEventFire]
        public void ClosePaymentSection(NodeRemoveEvent e, SingleNode<SectionComponent> section, [JoinAll] SingleNode<SelfUserComponent> user) {
            Debug.Log("PaymentSectionSystem.ClosePaymentSection");
            ScheduleEvent<ClosePaymentSectionEvent>(user);
        }

        [OnEventFire]
        public void LeavePayment(NodeAddedEvent e, SingleNode<ScreenComponent> screen, [JoinAll] SingleNode<UserInPaymentSectionComponent> session) {
            if (screen.component.GetComponent<PaymentScreen>() == null) {
                session.Entity.RemoveComponent<UserInPaymentSectionComponent>();
            }
        }

        [OnEventFire]
        public void Process(ProcessPaymentEvent e, PackNode pack, [JoinAll] Optional<SelectedMethodNode> methodNodeOptional) {
            long num = pack.xCrystalsPack.Amount;

            if (!pack.Entity.HasComponent<SpecialOfferComponent>()) {
                num = (long)Math.Round(pack.goods.SaleState.AmountMultiplier * num);
            }

            e.TotalAmount = num + pack.xCrystalsPack.Bonus;
        }

        [OnEventFire]
        public void Changed(GoodsChangedEvent e, SelectedPackNode selectedPack, [JoinAll] CurrentScreenNode screen) {
            ShowNotification(screen, selectedPack);
        }

        [OnEventFire]
        public void Changed(NodeRemoveEvent e, PersonalSpecialNode personal, [JoinBy(typeof(SpecialOfferGroupComponent))] SelectedSpecialPackNode selectedPack,
            [JoinAll] CurrentScreenNode screen) {
            ShowNotification(screen, selectedPack);
        }

        void ShowNotification(CurrentScreenNode screen, SelectedPackNode selectedPack) {
            PaymentScreen component = screen.screen.GetComponent<PaymentScreen>();

            if (component != null && component.GetType() != typeof(GoodsSelectionScreenComponent)) {
                ScheduleEvent<ShowScreenRightEvent<GoodsSelectionScreenComponent>>(selectedPack);
                Entity entity = CreateEntity<SaleEndNotificationTemplate>("notification/saleend");
                entity.AddComponent<NotificationComponent>();
            }
        }

        [OnEventFire]
        public void Destroy(NotificationShownEvent e, SingleNode<SaleEndNotificationTextComponent> node) {
            DeleteEntity(node.Entity);
        }

        [OnEventFire]
        public void MarkXCrystals(NodeAddedEvent e, SelfUserNode user, [Combine] SingleNode<ShopBadgeComponent> indicator, SingleNode<PaymentSpecialIconMinimalRankComponent> config) {
            indicator.component.NotificationAvailable = user.userRank.Rank >= int.Parse(config.component.MinimalRank);
        }

        [OnEventFire]
        public void SetBadge(NodeAddedEvent e, [Combine] SingleNode<SpecialOfferVisibleComponent> special, SingleNode<ShopBadgeComponent> indicator) {
            indicator.component.SpecialOfferAvailable = true;
        }

        [OnEventComplete]
        public void SetBadge(NodeRemoveEvent e, [Combine] SingleNode<SpecialOfferVisibleComponent> special, ICollection<SingleNode<SpecialOfferVisibleComponent>> specials,
            [JoinAll] SingleNode<ShopBadgeComponent> indicator) {
            if (!specials.ToList().Any(x => x.Entity != special.Entity)) {
                indicator.component.SpecialOfferAvailable = false;
            }
        }

        [OnEventFire]
        public void Click(ButtonClickEvent e, SingleNode<XCrystalsIndicatorComponent> indicator, [JoinAll] SelfUserNode user) {
            ScheduleEvent<GoToXCryShopScreen>(user);
        }

        public class SelectedPackNode : Node {
            public GoodsComponent goods;

            public SelectedListItemComponent selectedListItem;
        }

        public class PackNode : Node {
            public GoodsComponent goods;

            public XCrystalsPackComponent xCrystalsPack;
        }

        public class SelectedSpecialPackNode : SelectedPackNode {
            public new GoodsComponent goods;

            public new SelectedListItemComponent selectedListItem;

            public SpecialOfferComponent specialOffer;

            public SpecialOfferGroupComponent specialOfferGroup;
        }

        public class PersonalSpecialNode : Node {
            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferVisibleComponent specialOfferVisible;
        }

        public class CurrentScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public ScreenComponent screen;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }

        public class SelectedMethodNode : Node {
            public PaymentMethodComponent paymentMethod;

            public SelectedListItemComponent selectedListItem;
        }

        public class DestroyNotificationEvent : Event { }
    }
}