using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.API {
    [Shared]
    [SerialVersionUID(488563242347832343L)]
    public class LegendaryTankSpecialOfferComponent : Component {
        public RentTankRole TankRole { get; set; }

        public long MaxRank { get; set; }
    }
}