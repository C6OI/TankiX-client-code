using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1439792100478L)]
    public class SessionSecurityPublicComponent : Component {
        public SessionSecurityPublicComponent() { }

        public SessionSecurityPublicComponent(string publicKey) => PublicKey = publicKey;

        public string PublicKey { get; set; }
    }
}