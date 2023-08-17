using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientPayment.Impl {
    public class PaymentNotificationTextComponent : Component {
        public string MessageTemplate { get; set; }
    }
}