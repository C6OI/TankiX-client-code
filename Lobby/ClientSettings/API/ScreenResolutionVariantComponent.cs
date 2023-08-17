using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientSettings.API {
    public class ScreenResolutionVariantComponent : Component {
        public int Width { get; set; }

        public int Height { get; set; }
    }
}