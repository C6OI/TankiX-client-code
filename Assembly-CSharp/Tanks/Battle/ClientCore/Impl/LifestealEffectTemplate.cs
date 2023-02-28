using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636341525184122918L)]
    public interface LifestealEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        LifestealEffectComponent lifestealEffect();
    }
}