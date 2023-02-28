using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class LeagueNameComponent : Component {
        public string Name { get; set; }

        public string NameAccusative { get; set; }
    }
}