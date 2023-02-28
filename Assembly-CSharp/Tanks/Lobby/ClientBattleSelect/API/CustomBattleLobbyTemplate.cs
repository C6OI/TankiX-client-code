using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1498460950985L)]
    public interface CustomBattleLobbyTemplate : BattleLobbyTemplate, Template {
        [AutoAdded]
        CustomBattleLobbyComponent customBattleLobby();
    }
}