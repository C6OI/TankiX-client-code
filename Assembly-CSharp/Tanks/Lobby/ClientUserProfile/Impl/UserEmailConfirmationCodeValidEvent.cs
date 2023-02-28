using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1457951998279L)]
    public class UserEmailConfirmationCodeValidEvent : Event {
        public string Code { get; set; }
    }
}