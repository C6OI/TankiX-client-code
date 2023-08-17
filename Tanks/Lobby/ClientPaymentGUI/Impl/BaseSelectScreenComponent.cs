using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class BaseSelectScreenComponent : LocalizedScreenComponent, PaymentScreen {
        public IUIList List { get; private set; }

        protected override void Awake() {
            base.Awake();
            List = GetComponentInChildren<IUIList>();
        }
    }
}