using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class BattleSelectScreenContextComponent : Component {
        public BattleSelectScreenContextComponent() { }

        public BattleSelectScreenContextComponent(long? battleId) => BattleId = battleId;

        public long? BattleId { get; set; }
    }
}