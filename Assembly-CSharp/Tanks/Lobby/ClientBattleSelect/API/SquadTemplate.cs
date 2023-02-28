using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1507120664314L)]
    public interface SquadTemplate : Template {
        [AutoAdded]
        SquadComponent squad();

        [AutoAdded]
        [PersistentConfig]
        SquadConfigComponent squadConfig();
    }
}