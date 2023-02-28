using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class GrassSettingsComponent : Component {
        public int Value { get; set; }

        public float GrassNearDrawDistance { get; set; }

        public float GrassFarDrawDistance { get; set; }

        public float GrassFadeRange { get; set; }

        public float GrassDensityMultiplier { get; set; }

        public bool GrassCastsShadow { get; set; }
    }
}