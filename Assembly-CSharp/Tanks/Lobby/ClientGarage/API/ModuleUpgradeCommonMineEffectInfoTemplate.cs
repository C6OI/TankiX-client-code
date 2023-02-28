using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636364875302316714L)]
    public interface ModuleUpgradeCommonMineEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectImpactPropertyComponent moduleMineEffectImpactProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectHideRangePropertyComponent moduleMineEffectHideRangeProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectBeginHideDistancePropertyComponent moduleMineEffectBeginHideDistanceProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectActivationTimePropertyComponent moduleEffectActivationTimeProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectSplashDamageMaxRadiusPropertyComponent moduleMineEffectSplashDamageMaxRadiusProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectSplashDamageMinPercentPropertyComponent moduleMineEffectSplashDamageMinPercentProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectSplashDamageMinRadiusPropertyComponent moduleMineEffectSplashDamageMinRadiusProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleLimitBundleEffectCountPropertyComponent moduleLimitBundleEffectCountProperty();
    }
}