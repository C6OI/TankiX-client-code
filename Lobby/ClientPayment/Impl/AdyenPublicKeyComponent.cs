using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientPayment.Impl {
    public class AdyenPublicKeyComponent : FromConfigBehaviour, Component {
        public override string ConfigPath => "payment/provider/adyen";

        public string PublicKey { get; set; }
    }
}