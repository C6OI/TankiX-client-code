using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientPaymentGUI.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumLearnMoreComponent : ConfirmDialogComponent {
        public TabManager tabManager;

        public PremiumInfoUiElement[] uiElements;

        void Start() {
            GoToShopButton componentInChildren = GetComponentInChildren<GoToShopButton>();

            if (!(componentInChildren == null)) {
                componentInChildren.DesiredShopTab = 5;
                componentInChildren.CallDialog = this;
            }
        }
    }
}