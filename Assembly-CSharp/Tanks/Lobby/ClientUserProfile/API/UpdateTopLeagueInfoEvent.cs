using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1522324991586L)]
    public class UpdateTopLeagueInfoEvent : Event {
        public long UserId { get; set; }

        public double LastPlaceReputation { get; set; }

        public int Place { get; set; }
    }
}