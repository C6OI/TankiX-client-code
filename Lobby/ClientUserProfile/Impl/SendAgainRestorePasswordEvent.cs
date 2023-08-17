using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1460461200896L)]
    [Shared]
    public class SendAgainRestorePasswordEvent : Event { }
}