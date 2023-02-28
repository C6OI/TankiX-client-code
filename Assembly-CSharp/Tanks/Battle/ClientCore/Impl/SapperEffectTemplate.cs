using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1553764196292L)]
    public interface SapperEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        SapperEffectComponent sapperEffect();
    }
}