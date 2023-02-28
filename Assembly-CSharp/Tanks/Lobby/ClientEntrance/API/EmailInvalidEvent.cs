using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1455866538339L)]
    public class EmailInvalidEvent : Event {
        public string Email { get; set; }
    }
}