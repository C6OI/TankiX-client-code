using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class BundleContainerContentItemComponent : Component {
        public List<MarketItemBundle> MarketItems { get; set; }

        public string NameLokalizationKey { get; set; }
    }
}