using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentDataComponent : Component {
        public string PhoneNumber { get; set; }

        public string TransactionId { get; set; }
    }
}