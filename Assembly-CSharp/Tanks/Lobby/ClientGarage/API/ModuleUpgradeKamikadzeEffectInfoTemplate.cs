using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1554360394829L)]
    public interface ModuleUpgradeKamikadzeEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        ModuleKamikadzeEffectPropertyComponent moduleKamikadzeEFfectProperty();

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