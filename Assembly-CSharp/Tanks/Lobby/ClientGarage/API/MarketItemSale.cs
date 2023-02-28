using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class MarketItemSale {
        public int salePercent { get; set; }

        public int priceOffset { get; set; }

        public int xPriceOffset { get; set; }

        public Date endDate { get; set; }
    }
}