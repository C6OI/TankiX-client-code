using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1440486082251L)]
    public class EnterBattleAsSpectatorRequestEvent : Event { }
}