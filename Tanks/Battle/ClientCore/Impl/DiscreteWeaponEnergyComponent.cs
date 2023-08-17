using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1438077188268L)]
    [Shared]
    public class DiscreteWeaponEnergyComponent : Component {
        public float UnloadEnergyPerShot { get; set; }

        public float ReloadEnergyPerSec { get; set; }
    }
}