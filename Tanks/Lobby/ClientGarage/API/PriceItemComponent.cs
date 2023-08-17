using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433485953323L)]
    public class PriceItemComponent : Component {
        public int Price { get; set; }

        public bool IsBuyable => Price > 0;
    }
}