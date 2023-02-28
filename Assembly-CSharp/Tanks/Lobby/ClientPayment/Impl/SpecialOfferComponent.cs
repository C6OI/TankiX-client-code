using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636177018942961050L)]
    public class SpecialOfferComponent : Component {
        public double Discount { get; set; }

        public double GetSalePrice(double price) => Math.Round(price * (100.0 - Discount) / 100.0);
    }
}