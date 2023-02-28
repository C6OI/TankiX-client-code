using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class AnisotropicQualityVariantComponent : Component {
        public int Value { get; set; }

        public int AnisotropicFiltering { get; set; }
    }
}