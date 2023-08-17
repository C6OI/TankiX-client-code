using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(1464349204724L)]
    [Shared]
    public class ClientInfoSendEvent : Event {
        public ClientInfoSendEvent(string settings) => Settings = settings;

        public string Settings { get; set; }
    }
}