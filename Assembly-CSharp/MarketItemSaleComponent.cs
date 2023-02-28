using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

public class MarketItemSaleComponent : Component {
    public int salePercent { get; set; }

    public int priceOffset { get; set; }

    public int xPriceOffset { get; set; }

    public Date endDate { get; set; }
}