using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1439558339769L)]
    public interface ArmorPropertyTemplate : Template, SupplyPropertyTemplate {
        [AutoAdded]
        [PersistentConfig]
        ArmorEffectComponent armorEffect();

        [AutoAdded]
        [PersistentConfig("duration")]
        DurationConfigComponent durationConfig();
    }
}