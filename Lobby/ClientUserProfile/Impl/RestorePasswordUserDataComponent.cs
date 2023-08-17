using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1460458102410L)]
    public class RestorePasswordUserDataComponent : Component {
        public string Email { get; set; }

        public string Uid { get; set; }
    }
}