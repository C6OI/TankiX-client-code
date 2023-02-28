using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1496906087610L)]
    public class UserEquipmentComponent : Component {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}