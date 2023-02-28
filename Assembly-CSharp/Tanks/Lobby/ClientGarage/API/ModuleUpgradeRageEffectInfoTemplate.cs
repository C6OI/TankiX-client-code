using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636362459608393188L)]
    public interface ModuleUpgradeRageEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleRageEffectReduceCooldownTimePerKillPropertyComponent moduleRageEffectReduceCooldownTimePerKillProperty();
    }
}