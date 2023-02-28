using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class ShadowQualityVariantComponent : Component {
        public int Value { get; set; }

        public int ShadowQuality { get; set; }

        public int ShadowResolution { get; set; }

        public int ShadowProjection { get; set; }

        public int ShadowDistance { get; set; }

        public int ShadowNearPlaneOffset { get; set; }

        public int ShadowCascades { get; set; }

        public float ShadowCascade1 { get; set; }

        public float ShadowCascade2 { get; set; }

        public float ShadowCascade3 { get; set; }
    }
}