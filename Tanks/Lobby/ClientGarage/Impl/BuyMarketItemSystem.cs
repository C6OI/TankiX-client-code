using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPaymentGUI.Impl;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuyMarketItemSystem : ECSSystem {
        [OnEventFire]
        public void BuyItem(ConfirmButtonClickYesEvent e, ButtonNode buyItemButton,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem, [JoinAll] SingleNode<SelfUserComponent> user) {
            Entity selectedItem2 = selectedItem.component.SelectedItem;

            NewEvent(new BuyMarketItemEvent {
                Price = (int)buyItemButton.priceLabel.Price
            }).AttachAll(selectedItem2, user.Entity).Schedule();
        }

        [OnEventFire]
        public void CheckPrice(ConfirmButtonClickEvent e, ButtonNode buyItemButton, [JoinAll] SelfUserNode user,
            [JoinAll] SingleNode<DialogsComponent> dialogs, [JoinAll] SingleNode<ActiveScreenComponent> activeScreen) {
            if (user.userMoney.Money < buyItemButton.priceLabel.Price) {
                buyItemButton.confirmButton.FlipFront();

                dialogs.component.Get<NotEnoughCrystalsWindow>().ShowForCrystals(user.Entity,
                    activeScreen.Entity,
                    buyItemButton.priceLabel.Price - user.userMoney.Money);
            }
        }

        [OnEventFire]
        public void CheckXPrice(ConfirmButtonClickEvent e, XButtonNode buyItemButton, [JoinAll] SelfUserNode user,
            [JoinAll] SingleNode<DialogsComponent> dialogs, [JoinAll] SingleNode<ActiveScreenComponent> activeScreen) {
            if (user.userXCrystals.Money < buyItemButton.xPriceLabel.Price) {
                buyItemButton.confirmButton.FlipFront();

                dialogs.component.Get<NotEnoughCrystalsWindow>().ShowForXCrystals(user.Entity,
                    activeScreen.Entity,
                    buyItemButton.xPriceLabel.Price - user.userXCrystals.Money);
            }
        }

        [OnEventFire]
        public void XBuyItem(ConfirmButtonClickYesEvent e, XButtonNode buyItemButton,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem, [JoinAll] SelfUserNode user) {
            Entity selectedItem2 = selectedItem.component.SelectedItem;

            NewEvent(new XBuyMarketItemEvent {
                Price = (int)buyItemButton.xPriceLabel.Price
            }).AttachAll(selectedItem2, user.Entity).Schedule();
        }

        public class ButtonNode : Node {
            public BuyButtonComponent buyButton;

            public ConfirmButtonComponent confirmButton;
            public PriceLabelComponent priceLabel;
        }

        public class XButtonNode : Node {
            public BuyButtonComponent buyButton;

            public ConfirmButtonComponent confirmButton;
            public XPriceLabelComponent xPriceLabel;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserMoneyComponent userMoney;
            public UserXCrystalsComponent userXCrystals;
        }
    }
}