using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ApplyCameraTransformEvent : Event {
        public float? deltaTime;
        public float? positionSmoothingRatio;

        public float? rotationSmoothingRatio;
    }
}