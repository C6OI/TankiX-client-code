using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(-2900692155258405631L)]
    public class CooldownConfigComponent : Component {
        public int SelfCooldown { get; set; }

        public List<ConfigDependentCooldown> Dependencies { get; set; }
    }
}