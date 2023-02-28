using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636384699493258494L)]
    public interface ModuleUpgradeIcetrapEffectInfoTemplate : ModuleUpgradeMineEffectInfoTemplate, ModuleUpgradeCommonMineEffectInfoTemplate, ModuleUpgradeInfoTemplate, Template {
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