using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636362277898499977L)]
    public interface ModuleUpgradeEmergencyProtectionEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEmergencyProtectionEffectHolyshieldDurationPropertyComponent moduleEmergencyProtectionEffectHolyshieldDurationProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEmergencyProtectionEffectFixedHPPropertyComponent moduleEmergencyProtectionEffectFixedHPProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEmergencyProtectionEffectAdditiveHPFactorPropertyComponent moduleEmergencyProtectionEffectAdditiveHPFactorProperty();
    }
}