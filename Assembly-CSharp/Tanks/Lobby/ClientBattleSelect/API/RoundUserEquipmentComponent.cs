using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class RoundUserEquipmentComponent : Component {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}