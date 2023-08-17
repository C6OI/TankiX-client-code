using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [Shared]
    [SerialVersionUID(1446201736082L)]
    public class UserDetachFromSectionEvent : Event { }
}