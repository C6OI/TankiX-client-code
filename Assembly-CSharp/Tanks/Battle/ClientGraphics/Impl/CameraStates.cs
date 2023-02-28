using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class CameraStates {
        public class CameraFollowState : Node {
            public FollowCameraComponent followCamera;
        }

        public class CameraOrbitState : Node {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class CameraFreeState : Node {
            public FreeCameraComponent freeCamera;
        }

        public class CameraTransitionState : Node {
            public TransitionCameraStateComponent transitionCameraState;
        }

        public class CameraGoState : Node {
            public GoCameraComponent goCamera;
        }
    }
}