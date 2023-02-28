using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636459062089487176L)]
    public class UserDailyBonusZoneComponent : Component {
        public long ZoneNumber { get; set; }
    }
}