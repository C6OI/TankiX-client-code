using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1487227012386L)]
    public interface NormalizeTemperatureEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        NormalizeTemperatureEffectComponent normalizeTemperatureEffect();
    }
}