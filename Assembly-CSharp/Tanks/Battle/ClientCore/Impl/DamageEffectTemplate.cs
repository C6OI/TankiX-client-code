using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486018791920L)]
    public interface DamageEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        DamageEffectComponent damageEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}