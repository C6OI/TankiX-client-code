using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1499687865729L)]
    public class BattleLobbyEnterToBattleErrorEvent : Event { }
}