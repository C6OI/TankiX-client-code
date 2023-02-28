using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class OpenPersonalChannelEvent : Event {
        public string UserUid { get; set; }
    }
}