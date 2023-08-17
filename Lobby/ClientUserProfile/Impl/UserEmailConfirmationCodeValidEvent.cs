using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1457951998279L)]
    [Shared]
    public class UserEmailConfirmationCodeValidEvent : Event {
        public string Code { get; set; }
    }
}