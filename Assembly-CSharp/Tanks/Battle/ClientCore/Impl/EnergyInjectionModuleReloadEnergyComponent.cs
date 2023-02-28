using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636367507221863506L)]
    public class EnergyInjectionModuleReloadEnergyComponent : Component {
        public float ReloadEnergyPercent { get; set; }
    }
}