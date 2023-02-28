using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636350313583211976L)]
    public interface ModuleUpgradeInfoTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleCooldownPropertyComponent moduleCooldownProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleAmmunitionPropertyComponent moduleAmmunitionProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleVisualPropertiesComponent moduleVisualProperties();
    }
}