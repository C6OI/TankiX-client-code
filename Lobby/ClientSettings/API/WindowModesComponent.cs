using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientSettings.API {
    public class WindowModesComponent : Component {
        public string Fullscreen { get; set; }

        public string Windowed { get; set; }
    }
}