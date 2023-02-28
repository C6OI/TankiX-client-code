using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486970297039L)]
    public interface TurboSpeedEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        TurboSpeedEffectComponent turboSpeedEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}