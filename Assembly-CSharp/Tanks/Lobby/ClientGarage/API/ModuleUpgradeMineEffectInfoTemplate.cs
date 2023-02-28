using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636372750516278436L)]
    public interface ModuleUpgradeMineEffectInfoTemplate : ModuleUpgradeCommonMineEffectInfoTemplate, ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectTriggeringAreaPropertyComponent moduleMineEffectTriggeringAreaProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleMineEffectExplosionDelayMSPropertyComponent moduleMineEffectExplosionDelayMSProperty();
    }
}