using System.Collections.Generic;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PackPurchaseSystem : ECSSystem {
        [OnEventFire]
        public void ShowShopDialog(ButtonClickEvent e, PurchaseButtonNode node, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            ShopDialogs shopDialogs = dialogs.component.Get<ShopDialogs>();
            node.purchaseDialog.shopDialogs = shopDialogs;
            node.purchaseDialog.ShowDialog(node.purchaseButton.GoodsEntity);
        }

        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, PurchaseButtonNode node, [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods) {
            foreach (SingleNode<PaymentMethodComponent> method in methods) {
                node.purchaseDialog.AddMethod(method.Entity);
            }
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<PurchaseDialogComponent> dialog) {
            dialog.component.Clear();
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> steam, [Context] [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs) {
            foreach (SingleNode<PurchaseDialogComponent> dialog in dialogs) {
                dialog.component.SteamComponentIsPresent = true;
            }
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> payment, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs) {
            foreach (SingleNode<PurchaseDialogComponent> dialog in dialogs) {
                dialog.component.HandleSuccessMobile(e.TransactionId);
            }
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node any, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs) {
            foreach (SingleNode<PurchaseDialogComponent> dialog in dialogs) {
                dialog.component.HandleQiwiError();
            }
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> payment, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs) {
            foreach (SingleNode<PurchaseDialogComponent> dialog in dialogs) {
                dialog.component.HandleError();
            }
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> payment, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs) {
            foreach (SingleNode<PurchaseDialogComponent> dialog in dialogs) {
                dialog.component.HandleSuccess();
            }
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs) {
            foreach (SingleNode<PurchaseDialogComponent> dialog in dialogs) {
                dialog.component.HandleGoToLink();
            }
        }

        public class PurchaseButtonNode : Node {
            public PurchaseButtonComponent purchaseButton;

            public PurchaseDialogComponent purchaseDialog;
        }
    }
}