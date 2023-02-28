using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [Shared]
    [SerialVersionUID(1504007285141L)]
    public class EnteredToMatchMakingEvent : Event { }
}