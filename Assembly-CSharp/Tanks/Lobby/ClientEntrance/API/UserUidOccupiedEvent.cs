using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1437991652726L)]
    public class UserUidOccupiedEvent : Event {
        public string Uid { get; set; }
    }
}