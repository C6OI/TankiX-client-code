using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1458034009617L)]
    public class RequestSendAgainConfirmationEmailEvent : Event { }
}