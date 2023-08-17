using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1457951468449L)]
    [Shared]
    public class RequestUserEmailConfirmationEvent : Event {
        public string Code { get; set; }

        public bool Subscribe { get; set; }
    }
}