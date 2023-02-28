using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636222384398205627L)]
    public interface InvisibilityEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        InvisibilityEffectComponent invisibilityEffect();
    }
}