using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentProcessingScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen) => screen.Entity.AddComponent<LockedScreenComponent>();

        public class ScreenNode : Node {
            public PaymentProcessingScreenComponent paymentProcessingScreen;
        }
    }
}