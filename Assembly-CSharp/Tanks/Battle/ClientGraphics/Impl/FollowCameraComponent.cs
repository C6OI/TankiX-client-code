using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FollowCameraComponent : Component {
        public CameraData cameraData;

        public float rollReturnSpeedDegPerSec = 60f;
        public float verticalCameraSpeed = 0.5f;
    }
}