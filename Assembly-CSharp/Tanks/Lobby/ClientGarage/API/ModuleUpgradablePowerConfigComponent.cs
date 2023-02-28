using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636329485160809136L)]
    public class ModuleUpgradablePowerConfigComponent : Component {
        public List<List<int>> Level2PowerByTier { get; set; }
    }
}