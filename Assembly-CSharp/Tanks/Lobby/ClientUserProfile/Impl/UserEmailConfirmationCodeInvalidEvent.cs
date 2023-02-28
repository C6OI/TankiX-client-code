using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1457951918247L)]
    public class UserEmailConfirmationCodeInvalidEvent : Event {
        public string Code { get; set; }
    }
}