using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class XPriceItemComponent : Component {
        public int Pieces { get; set; }

        public int OldPrice { get; set; }

        public int Price { get; set; }

        [ProtocolTransient] public bool IsBuyable => Price > 0;
    }
}