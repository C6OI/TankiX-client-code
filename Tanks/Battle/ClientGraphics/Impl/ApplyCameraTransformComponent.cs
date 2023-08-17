using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ApplyCameraTransformComponent : Component {
        public float positionSmoothingRatio = 1f;

        public float rotationSmoothingRatio = 1f;
    }
}