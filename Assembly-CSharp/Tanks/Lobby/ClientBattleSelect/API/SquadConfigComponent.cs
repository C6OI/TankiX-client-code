using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class SquadConfigComponent : Component {
        public int MaxSquadSize { get; set; }
    }
}