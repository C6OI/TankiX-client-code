using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1505906112954L)]
    public class LeagueFirstEntranceRewardNotificationComponent : Component {
        public Dictionary<long, int> Reward { get; set; }
    }
}