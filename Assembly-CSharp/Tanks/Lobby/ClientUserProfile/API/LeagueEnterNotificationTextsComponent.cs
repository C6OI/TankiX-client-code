using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class LeagueEnterNotificationTextsComponent : Component {
        public string HeaderText { get; set; }

        public string Text { get; set; }
    }
}