using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1502886877871L)]
    public class PresetEquipmentComponent : Component {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}