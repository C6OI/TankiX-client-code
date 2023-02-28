using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636469075744282523L)]
    public class KickUserFromChatEvent : Event {
        public string UserUid { get; set; }
    }
}