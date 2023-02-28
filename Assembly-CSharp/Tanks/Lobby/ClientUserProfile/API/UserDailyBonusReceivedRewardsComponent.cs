using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636459174909060087L)]
    public class UserDailyBonusReceivedRewardsComponent : Component {
        public List<long> ReceivedRewards { get; set; }
    }
}