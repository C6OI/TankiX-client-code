using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class CurrentSeasonNameComponent : Component {
        public string SeasonName { get; set; }
    }
}