using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1460106433434L)]
    public class RestorePasswordByEmailEvent : Event {
        public string Email { get; set; }
    }
}