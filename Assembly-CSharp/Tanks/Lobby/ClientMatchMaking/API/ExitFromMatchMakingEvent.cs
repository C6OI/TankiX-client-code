using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [Shared]
    [SerialVersionUID(1495176527022L)]
    public class ExitFromMatchMakingEvent : Event {
        public bool InBattle { get; set; }
    }
}