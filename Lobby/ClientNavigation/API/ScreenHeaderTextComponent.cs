using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class ScreenHeaderTextComponent : Component {
        public ScreenHeaderTextComponent() { }

        public ScreenHeaderTextComponent(string headerText) => HeaderText = headerText;

        public string HeaderText { get; set; }
    }
}