using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [Shared]
    [SerialVersionUID(1499773462695L)]
    public class MatchMakingLobbyStartingEvent : Event { }
}