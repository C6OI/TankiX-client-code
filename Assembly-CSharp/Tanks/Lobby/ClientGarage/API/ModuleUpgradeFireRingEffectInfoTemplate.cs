using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API.FireTrap;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1542699020893L)]
    public interface ModuleUpgradeFireRingEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        ModuleFireRingEffectPropertyComponent moduleFireRingEffectProperty();

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

        [AutoAdded]
        [PersistentConfig]
        ModuleIcetrapEffectTemperatureDurationPropertyComponent moduleIcetrapEffectTemperatureDurationProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleIcetrapEffectTemperatureDeltaPropertyComponent moduleIcetrapEffectTemperatureDeltaProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleIcetrapEffectTemperatureLimitPropertyComponent moduleIcetrapEffectTemperatureLimitProperty();
    }
}