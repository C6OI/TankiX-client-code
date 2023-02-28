using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ItemPacksComponent : Component {
        public List<int> ForXPrice { get; set; }

        public List<int> ForPrice { get; set; }
    }
}