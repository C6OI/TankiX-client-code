using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1439558350615L)]
    public interface DamagePropertyTemplate : Template, SupplyPropertyTemplate {
        [PersistentConfig]
        [AutoAdded]
        DamageEffectComponent damageEffect();

        [PersistentConfig("duration")]
        [AutoAdded]
        DurationConfigComponent durationConfig();
    }
}