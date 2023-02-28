using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1503314606668L)]
    public interface ForceFieldEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ForceFieldEffectComponent forceFieldEffect();
    }
}