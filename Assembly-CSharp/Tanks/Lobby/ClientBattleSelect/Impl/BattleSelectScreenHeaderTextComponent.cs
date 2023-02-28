using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectScreenHeaderTextComponent : Component {
        public string HeaderText { get; set; }

        public float HeaderTextShowDelaySeconds { get; set; }
    }
}