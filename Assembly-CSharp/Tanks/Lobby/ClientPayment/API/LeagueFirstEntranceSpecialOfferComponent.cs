using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.API {
    public class LeagueFirstEntranceSpecialOfferComponent : Component {
        public long LeagueId { get; set; }

        public int WorthItPercent { get; set; }
    }
}