using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1438077278464L)]
    public class StreamWeaponEnergyComponent : Component {
        public float UnloadEnergyPerSec { get; set; }

        public float ReloadEnergyPerSec { get; set; }
    }
}