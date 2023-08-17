using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1473403050079L)]
    public class XPriceItemComponent : Component {
        public int Price { get; set; }

        public bool IsBuyable => Price > 0;
    }
}