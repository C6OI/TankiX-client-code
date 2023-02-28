using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1547017351411L)]
    public class FractionsCompetitionRewardNotificationComponent : Component {
        public long WinnerFractionId { get; set; }

        public long CrysForWin { get; set; }

        public Dictionary<long, int> Rewards { get; set; }
    }
}