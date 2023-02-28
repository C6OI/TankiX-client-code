using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1464349204724L)]
    public class ClientInfoSendEvent : Event {
        public ClientInfoSendEvent(string settings) => Settings = settings;

        public string Settings { get; set; }
    }
}