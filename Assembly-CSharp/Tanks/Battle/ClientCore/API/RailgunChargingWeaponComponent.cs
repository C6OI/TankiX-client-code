using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(2654416098660377118L)]
    public class RailgunChargingWeaponComponent : Component {
        public float ChargingTime { get; set; }
    }
}