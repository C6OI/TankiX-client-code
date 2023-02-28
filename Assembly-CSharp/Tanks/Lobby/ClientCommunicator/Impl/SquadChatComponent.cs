using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(636475134000101195L)]
    public class SquadChatComponent : Component {
        public ChatType ChatType => ChatType.SQUAD;
    }
}