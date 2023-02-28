using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486018775542L)]
    public interface ArmorEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        ArmorEffectComponent armorEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}