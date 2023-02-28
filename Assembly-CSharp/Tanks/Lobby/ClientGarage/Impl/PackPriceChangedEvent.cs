using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1526939005944L)]
    public class PackPriceChangedEvent : Event {
        public Dictionary<int, int> PackPrice { get; set; }

        public Dictionary<int, int> PackXPrice { get; set; }

        public Dictionary<int, int> OldPackPrice { get; set; }

        public Dictionary<int, int> OldPackXPrice { get; set; }
    }
}