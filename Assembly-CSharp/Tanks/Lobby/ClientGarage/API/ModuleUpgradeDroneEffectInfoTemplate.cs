using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635453745672535457L)]
    public interface ModuleUpgradeDroneEffectInfoTemplate : ModuleUpgradeEffectWithDurationTemplate, ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEffectTargetingDistancePropertyComponent moduleEffectTargetingDistanceProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectTargetingPeriodPropertyComponent moduleEffectTargetingPeriodProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectActivationTimePropertyComponent moduleEffectActivationTimeProperty();
    }
}