using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636469079584474098L)]
    public class CreateCustomChatEvent : Event { }
}