using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class CustomOfferPriceForUIComponent : Component {
        public CustomOfferPriceForUIComponent(double price) => Price = price;

        public double Price { get; }
    }
}