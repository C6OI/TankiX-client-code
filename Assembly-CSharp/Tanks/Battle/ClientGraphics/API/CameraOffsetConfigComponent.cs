using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class CameraOffsetConfigComponent : Component {
        public float XOffset { get; set; }

        public float YOffset { get; set; }

        public float ZOffset { get; set; }
    }
}