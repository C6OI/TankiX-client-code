using Lobby.ClientControls.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentCheckoutScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen,
            [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment, [JoinAll] SelectedPackNode selectedPack) {
            DeleteEntity(mobilePayment.Entity);
            screen.component.SetTransactionNumber(mobilePayment.component.TransactionId);
            screen.component.SetPhoneNumber(mobilePayment.component.PhoneNumber);
            screen.component.SetCrystalsAmount(selectedPack.xCrystalsPack.Total);
            screen.component.SetPrice(selectedPack.goodsPrice.Price, selectedPack.goodsPrice.Currency);
        }

        public class SelectedPackNode : Node {
            public GoodsPriceComponent goodsPrice;
            public SelectedListItemComponent selectedListItem;

            public XCrystalsPackComponent xCrystalsPack;
        }
    }
}