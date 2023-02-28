using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1503304746220L)]
    public class LeagueEnergyConfigComponent : Component {
        public long Cost { get; set; }

        public long Capacity { get; set; }
    }
}