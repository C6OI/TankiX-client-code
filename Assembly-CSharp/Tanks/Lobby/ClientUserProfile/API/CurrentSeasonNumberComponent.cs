using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1508823738925L)]
    public class CurrentSeasonNumberComponent : Component {
        public int SeasonNumber { get; set; }
    }
}