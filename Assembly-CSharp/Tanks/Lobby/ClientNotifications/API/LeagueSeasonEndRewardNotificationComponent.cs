using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1508753065310L)]
    public class LeagueSeasonEndRewardNotificationComponent : Component {
        public int SeasonNumber { get; set; }

        public long LeagueId { get; set; }

        public Dictionary<long, int> Reward { get; set; }
    }
}