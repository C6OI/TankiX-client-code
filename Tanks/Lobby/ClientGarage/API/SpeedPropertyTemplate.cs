using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1439558377774L)]
    public interface SpeedPropertyTemplate : Template, SupplyPropertyTemplate {
        [AutoAdded]
        [PersistentConfig]
        SpeedEffectComponent speedEffect();

        [PersistentConfig("duration")]
        [AutoAdded]
        DurationConfigComponent durationConfig();
    }
}