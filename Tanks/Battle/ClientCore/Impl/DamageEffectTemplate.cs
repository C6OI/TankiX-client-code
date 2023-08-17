using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-170679271168992331L)]
    public interface DamageEffectTemplate : Template, EffectTemplate {
        [PersistentConfig]
        DamageEffectComponent damageEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}