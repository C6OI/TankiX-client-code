using System.Collections.Generic;

namespace Tanks.Lobby.ClientGarage.API {
    public class ContainerItem {
        public List<MarketItemBundle> ItemBundles { get; set; }

        public long Compensation { get; set; }

        public string NameLocalizationKey { get; set; }
    }
}