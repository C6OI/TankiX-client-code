using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(636147227222284613L)]
    public class UserRankRewardNotificationInfoComponent : Component {
        public long RedCrystals { get; set; }

        public long BlueCrystals { get; set; }

        public long Rank { get; set; }
    }
}