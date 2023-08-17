using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [SerialVersionUID(1467022882740L)]
    [Shared]
    public class PaymentNotificationComponent : Component {
        public long Amount { get; set; }
    }
}