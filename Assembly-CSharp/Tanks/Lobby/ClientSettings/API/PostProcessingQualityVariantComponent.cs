using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class PostProcessingQualityVariantComponent : Component {
        public bool CustomSettings { get; set; }

        public bool AmbientOcclusion { get; set; }

        public bool Bloom { get; set; }

        public bool ChromaticAberration { get; set; }

        public bool Grain { get; set; }

        public bool Vignette { get; set; }

        public bool DisableBattleNotifications { get; set; }
    }
}