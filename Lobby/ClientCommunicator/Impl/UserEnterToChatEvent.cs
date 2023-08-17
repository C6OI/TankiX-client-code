using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1446719851802L)]
    public class UserEnterToChatEvent : Event { }
}