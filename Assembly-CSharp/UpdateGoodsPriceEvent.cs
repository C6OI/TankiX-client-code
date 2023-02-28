using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

[Shared]
[SerialVersionUID(1623342142134L)]
public class UpdateGoodsPriceEvent : Event {
    public string Currency { get; set; }

    public double Price { get; set; }

    public float DiscountCoeff { get; set; }
}