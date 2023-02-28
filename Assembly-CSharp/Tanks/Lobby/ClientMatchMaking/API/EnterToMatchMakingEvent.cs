using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [Shared]
    [SerialVersionUID(1494937115182L)]
    public class EnterToMatchMakingEvent : Event { }
}