using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1437991652726L)]
    [Shared]
    public class UserUidOccupiedEvent : Event {
        public string Uid { get; set; }
    }
}