using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentGiftComponent : Component {
        public PaymentGiftComponent(long gift) => Gift = gift;

        public long Gift { get; }
    }
}