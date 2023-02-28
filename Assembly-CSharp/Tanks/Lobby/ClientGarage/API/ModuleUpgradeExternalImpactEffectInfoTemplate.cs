using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1542275711926L)]
    public interface ModuleUpgradeExternalImpactEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        ModuleExternalImpactEffectPropertyComponent moduleExternalImpactEffectProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectSplashRadiusPropertyComponent moduleEffectSplashRadiusProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectSplashDamageMinPercentPropertyComponent moduleEffectSplashDamageMinPercentProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectImpactPropertyComponent moduleEffectImpactProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();
    }
}