using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636353594759166730L)]
    public interface OverhaulEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        HealingEffectComponent healingEffect();
    }
}