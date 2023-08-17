using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1453874218410L)]
    public class PaymentMethodComponent : Component {
        public string ProviderName { get; set; }

        public string MethodName { get; set; }
    }
}