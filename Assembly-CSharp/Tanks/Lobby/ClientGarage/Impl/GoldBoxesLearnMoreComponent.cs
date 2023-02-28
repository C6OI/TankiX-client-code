using Tanks.Lobby.ClientPaymentGUI.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBoxesLearnMoreComponent : ConfirmDialogComponent {
        void Start() {
            GoToShopButton componentInChildren = GetComponentInChildren<GoToShopButton>();

            if (!(componentInChildren == null)) {
                componentInChildren.DesiredShopTab = 6;
                componentInChildren.CallDialog = this;
            }
        }
    }
}