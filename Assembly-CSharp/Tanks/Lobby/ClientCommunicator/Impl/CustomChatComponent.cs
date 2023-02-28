using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(636439724009730014L)]
    public class CustomChatComponent : Component {
        public ChatType ChatType => ChatType.CUSTOMGROUP;
    }
}