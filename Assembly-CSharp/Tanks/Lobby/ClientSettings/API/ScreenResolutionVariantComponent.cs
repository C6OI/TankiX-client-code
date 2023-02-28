using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class ScreenResolutionVariantComponent : Component {
        public int Width { get; set; }

        public int Height { get; set; }
    }
}