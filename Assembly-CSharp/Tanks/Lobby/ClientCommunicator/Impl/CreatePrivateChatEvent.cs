using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636469080057216111L)]
    public class CreatePrivateChatEvent : Event {
        public string UserUid { get; set; }
    }
}