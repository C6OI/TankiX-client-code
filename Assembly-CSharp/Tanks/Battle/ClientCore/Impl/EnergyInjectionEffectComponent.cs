using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636367475685199712L)]
    public class EnergyInjectionEffectComponent : Component {
        public float ReloadEnergyPercent { get; set; }
    }
}