using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486615272464L)]
    public interface TemperatureEffectTemplate : EffectBaseTemplate, Template {
        [PersistentConfig]
        TemperatureEffectComponent temperatureEffect();
    }
}