using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [Shared]
    [SerialVersionUID(1509109822442L)]
    public class ExitedFromMatchMakingEvent : Event {
        public bool SelfAction { get; set; }
    }
}