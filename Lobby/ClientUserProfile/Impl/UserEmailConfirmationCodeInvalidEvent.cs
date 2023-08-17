using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1457951918247L)]
    [Shared]
    public class UserEmailConfirmationCodeInvalidEvent : Event {
        public string Code { get; set; }
    }
}