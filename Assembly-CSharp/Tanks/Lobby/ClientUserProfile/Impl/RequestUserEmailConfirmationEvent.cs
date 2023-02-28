using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1457951468449L)]
    public class RequestUserEmailConfirmationEvent : Event {
        public string Code { get; set; }
    }
}