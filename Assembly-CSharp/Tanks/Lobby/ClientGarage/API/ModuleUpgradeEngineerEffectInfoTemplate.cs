using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636353723186437756L)]
    public interface ModuleUpgradeEngineerEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEngineerEffectDurationFactorPropertyComponent moduleEngineerEffectDurationFactorProperty();
    }
}