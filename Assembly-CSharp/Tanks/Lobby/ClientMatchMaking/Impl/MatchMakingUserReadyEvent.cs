using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [Shared]
    [SerialVersionUID(1496829083447L)]
    public class MatchMakingUserReadyEvent : Event { }
}