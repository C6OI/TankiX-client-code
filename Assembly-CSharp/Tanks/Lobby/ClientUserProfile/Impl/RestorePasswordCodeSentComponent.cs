using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1479198715562L)]
    public class RestorePasswordCodeSentComponent : Component {
        public string Email { get; set; }
    }
}