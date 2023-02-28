using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1523423182023L)]
    public class LoginRewardsNotificationComponent : Component {
        public Dictionary<long, int> Reward { get; set; }

        public List<LoginRewardItem> AllReward { get; set; }

        public int CurrentDay { get; set; }
    }
}