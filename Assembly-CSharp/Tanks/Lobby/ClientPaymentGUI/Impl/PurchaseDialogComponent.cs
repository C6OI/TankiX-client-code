using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PurchaseDialogComponent : PurchaseItemComponent {
        public void ShowDialog(Entity goodsEntity) {
            OnPackClick(goodsEntity);
        }

        public void Clear() {
            shopDialogs = null;
            methods.Clear();
        }
    }
}