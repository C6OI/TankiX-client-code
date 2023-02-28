using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636364878050935974L)]
    public interface ModuleUpgradeSpiderMineEffectInfoTemplate : ModuleUpgradeCommonMineEffectInfoTemplate, ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEffectAccelerationPropertyComponent moduleEffectAccelerationProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleSpiderMineEffectChasingEnergyDrainRatePropertyComponent moduleSpiderMineEffectChasingEnergyDrainRateProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleSpiderMineEffectEnergyPropertyComponent moduleSpiderMineEffectEnergyProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleSpiderMineEffectIdleEnergyDrainRatePropertyComponent moduleSpiderMineEffectIdleEnergyDrainRateProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectSpeedPropertyComponent moduleEffectSpeedProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectTargetingDistancePropertyComponent moduleEffectTargetingDistanceProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectTargetingPeriodPropertyComponent moduleEffectTargetingPeriodProperty();
    }
}