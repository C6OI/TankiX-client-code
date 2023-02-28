using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.API {
    [SerialVersionUID(1513676025873L)]
    public class PremiumOfferComponent : Component {
        public int MinRank { get; set; }

        public int MaxRank { get; set; }
    }
}