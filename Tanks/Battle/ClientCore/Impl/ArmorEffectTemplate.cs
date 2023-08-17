using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(5334512544740273891L)]
    public interface ArmorEffectTemplate : Template, EffectTemplate {
        [PersistentConfig]
        ArmorEffectComponent armorEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}