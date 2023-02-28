using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(635906273700499964L)]
    public class EmailVacantEvent : Event {
        public string Email { get; set; }
    }
}