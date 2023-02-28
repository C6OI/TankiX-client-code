using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HealthFeedbackCameraPreparedComponent : Component {
        public HealthFeedbackCameraPreparedComponent(HealthFeedbackPostEffect healthFeedbackPostEffect) => HealthFeedbackPostEffect = healthFeedbackPostEffect;

        public float TargetIntensity { get; set; }

        public HealthFeedbackPostEffect HealthFeedbackPostEffect { get; set; }
    }
}