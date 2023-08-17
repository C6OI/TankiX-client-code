using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(635906273700499964L)]
    [Shared]
    public class EmailVacantEvent : Event {
        public string Email { get; set; }
    }
}