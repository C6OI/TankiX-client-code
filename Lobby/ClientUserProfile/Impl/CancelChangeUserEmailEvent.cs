using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1458665658716L)]
    public class CancelChangeUserEmailEvent : Event { }
}