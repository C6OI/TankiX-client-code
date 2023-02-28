using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636329494806190820L)]
    public interface ModuleTierConfigTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig();
    }
}