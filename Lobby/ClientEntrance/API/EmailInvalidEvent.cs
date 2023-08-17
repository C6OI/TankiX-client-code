using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1455866538339L)]
    [Shared]
    public class EmailInvalidEvent : Event {
        public string Email { get; set; }
    }
}