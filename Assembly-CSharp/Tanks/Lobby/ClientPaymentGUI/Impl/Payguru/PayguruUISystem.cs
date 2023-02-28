using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.Payguru {
    public class PayguruUISystem : ECSSystem {
        [OnEventFire]
        public void ShowOrderIdWindow(PayguruShareOrderEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.Get<PayguruDialogComponent>().gameObject.SetActive(true);
            dialogs.component.Get<PayguruDialogComponent>().setOrderId(e.Order);
            dialogs.component.Get<PayguruDialogComponent>().setBanksData(e.BanksInfo);
        }
    }
}