using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ScreenHeaderTextComponent : Component {
        public ScreenHeaderTextComponent() { }

        public ScreenHeaderTextComponent(string headerText) => HeaderText = headerText;

        public string HeaderText { get; set; }
    }
}