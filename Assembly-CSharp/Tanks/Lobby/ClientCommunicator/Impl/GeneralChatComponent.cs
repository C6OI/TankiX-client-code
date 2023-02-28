using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636439592303211297L)]
    public class GeneralChatComponent : Component {
        [ProtocolTransient] public ChatType ChatType => ChatType.GENERAL;
    }
}