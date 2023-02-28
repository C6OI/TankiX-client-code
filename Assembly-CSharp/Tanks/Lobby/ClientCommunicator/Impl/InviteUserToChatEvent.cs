using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636469074545384579L)]
    public class InviteUserToChatEvent : Event {
        public string UserUid { get; set; }
    }
}