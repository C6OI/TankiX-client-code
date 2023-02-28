using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636469076339876375L)]
    public class MuteUserEvent : Event {
        public string UserUid { get; set; }
    }
}