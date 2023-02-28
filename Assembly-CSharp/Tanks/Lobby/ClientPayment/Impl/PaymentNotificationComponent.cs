using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1467022882740L)]
    public class PaymentNotificationComponent : Component {
        public long Amount { get; set; }
    }
}