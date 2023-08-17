using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1439792100478L)]
    [Shared]
    public class SessionSecurityPublicComponent : Component {
        public SessionSecurityPublicComponent() { }

        public SessionSecurityPublicComponent(string publicKey) => PublicKey = publicKey;

        public string PublicKey { get; set; }
    }
}