using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SetTransparencyTransitionDataEvent : Event {
        public SetTransparencyTransitionDataEvent(float originAlpha, float targetAlpha, float transparencyTransitionTime) {
            OriginAlpha = originAlpha;
            TargetAlpha = targetAlpha;
            TransparencyTransitionTime = transparencyTransitionTime;
        }

        public float OriginAlpha { get; set; }

        public float TargetAlpha { get; set; }

        public float TransparencyTransitionTime { get; set; }
    }
}