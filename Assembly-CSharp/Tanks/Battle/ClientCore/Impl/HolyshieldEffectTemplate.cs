using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636342532571262167L)]
    public interface HolyshieldEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        HolyshieldEffectComponent holyshieldEffect();
    }
}