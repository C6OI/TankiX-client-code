using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486978694968L)]
    public interface AcceleratedGearsEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        AcceleratedGearsEffectComponent acceleratedGearsEffect();
    }
}