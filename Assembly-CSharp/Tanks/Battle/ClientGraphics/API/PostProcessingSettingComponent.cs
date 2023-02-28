using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class PostProcessingSettingComponent : Component {
        public bool Antialiasing { get; set; }

        public int AntialiasingPresset { get; set; }

        public bool AmbientOcclusion { get; set; }

        public bool ScreenSpaceReflection { get; set; }

        public bool DepthOfField { get; set; }

        public bool MotionBlur { get; set; }

        public bool EyeAdaptation { get; set; }

        public bool Bloom { get; set; }

        public bool ColorGrading { get; set; }

        public bool UserLut { get; set; }

        public bool ChromaticAberration { get; set; }

        public bool Grain { get; set; }

        public bool Vignette { get; set; }

        public bool Fog { get; set; }

        public bool TargetBloom { get; set; }
    }
}