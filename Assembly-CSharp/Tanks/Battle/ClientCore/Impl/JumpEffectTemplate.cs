using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1538451741218L)]
    public interface JumpEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        JumpEffectComponent jumpEffect();
    }
}