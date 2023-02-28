using Platform.Library.ClientUnityIntegration.API;
using TMPro;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class CustomDiscountUIComponent : BehaviourComponent {
        public TextMeshProUGUI description;

        public void OnDisable() {
            description.text = string.Empty;
        }
    }
}