using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.API {
    public class GoldBonusOfferComponent : Component {
        public long Count { get; set; }
    }
}