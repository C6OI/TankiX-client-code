using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636352947010929322L)]
    public interface ModuleUpgradeHealingEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleHealingEffectHPPerMSPropertyComponent moduleHealingEffectHPPerMSProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleHealingEffectInstantHPPropertyComponent moduleHealingEffectInstantHPProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleHealingEffectFirstTickMSPropertyComponent moduleHealingEffectFirstTickMsProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleHealingEffectPeriodicTickPropertyComponent moduleHealingEffectPeriodicTickProperty();
    }
}