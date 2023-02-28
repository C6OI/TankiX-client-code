using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636353660984440005L)]
    public interface ModuleUpgradeTempblockEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleTempblockDecrementPropertyComponent moduleTempblockDecrementProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleTempblockIncrementPropertyComponent moduleTempblockIncrementProperty();
    }
}