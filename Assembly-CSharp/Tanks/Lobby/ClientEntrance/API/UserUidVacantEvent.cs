using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1437991666522L)]
    public class UserUidVacantEvent : Event {
        public string Uid { get; set; }
    }
}