using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(635906273457089964L)]
    public class EmailOccupiedEvent : Event {
        public string Email { get; set; }
    }
}