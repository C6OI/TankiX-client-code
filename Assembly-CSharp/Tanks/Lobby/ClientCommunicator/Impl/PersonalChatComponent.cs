using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(636470402081671484L)]
    public class PersonalChatComponent : Component {
        public ChatType ChatType => ChatType.PERSONAL;
    }
}