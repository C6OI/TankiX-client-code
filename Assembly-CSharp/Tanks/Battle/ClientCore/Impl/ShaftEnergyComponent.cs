using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1826384779893027508L)]
    public class ShaftEnergyComponent : Component {
        public float UnloadEnergyPerQuickShot { get; set; }

        public float PossibleUnloadEnergyPerAimingShot { get; set; }

        public float UnloadAimingEnergyPerSec { get; set; }

        public float ReloadEnergyPerSec { get; set; }
    }
}