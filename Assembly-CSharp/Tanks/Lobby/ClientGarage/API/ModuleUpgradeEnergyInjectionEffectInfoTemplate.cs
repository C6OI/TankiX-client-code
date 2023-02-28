using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636367449583276768L)]
    public interface ModuleUpgradeEnergyInjectionEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEnergyInjectionEffectReloadPercentPropertyComponent moduleEnergyInjectionEffectReloadPercentProperty();
    }
}