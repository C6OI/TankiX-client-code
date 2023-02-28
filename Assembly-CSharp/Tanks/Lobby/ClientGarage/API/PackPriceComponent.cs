using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class PackPriceComponent : Component {
        public Dictionary<int, int> PackPrice { get; set; }

        public Dictionary<int, int> PackXPrice { get; set; }
    }
}