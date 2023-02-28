using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486988156885L)]
    public interface HealingEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        HealingEffectComponent healingEffect();
    }
}