using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636469076535361064L)]
    public class UnmuteUserEvent : Event {
        public string UserUid { get; set; }
    }
}