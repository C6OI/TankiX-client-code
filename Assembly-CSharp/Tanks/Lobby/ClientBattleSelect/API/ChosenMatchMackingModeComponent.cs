using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ChosenMatchMackingModeComponent : Component {
        public Entity ModeEntity { get; set; }
    }
}