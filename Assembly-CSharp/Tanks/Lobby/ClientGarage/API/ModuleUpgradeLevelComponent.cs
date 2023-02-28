using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636329487716905336L)]
    public class ModuleUpgradeLevelComponent : Component {
        public long Level { get; set; }
    }
}