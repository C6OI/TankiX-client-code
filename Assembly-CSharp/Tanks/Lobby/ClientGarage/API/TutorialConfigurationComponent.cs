using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1503999560012L)]
    public class TutorialConfigurationComponent : Component {
        public List<string> Tutorials { get; set; }
    }
}