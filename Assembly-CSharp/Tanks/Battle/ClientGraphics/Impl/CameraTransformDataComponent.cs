using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraTransformDataComponent : Component {
        public TransformData Data { get; set; }
    }
}