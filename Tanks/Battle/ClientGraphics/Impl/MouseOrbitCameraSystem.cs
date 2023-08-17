using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MouseOrbitCameraSystem : ECSSystem {
        const int DINSTANCE_RATIO_BASE = 3;

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void UpdateMouseOrbitCamera(TimeUpdateEvent evt, OrbitCameraNode cameraNode,
            [JoinAll] CameraTargetNode targetNode) {
            float deltaTime = evt.DeltaTime;
            MouseOrbitCameraComponent mouseOrbitCamera = cameraNode.mouseOrbitCamera;
            Vector3 eulerAngles = mouseOrbitCamera.targetRotation.eulerAngles;
            float num = 1f;

            if (InputManager.CheckAction(SpectatorCameraActions.SlowMovement)) {
                num = MouseOrbitCameraConstants.ROTATION_SLOW_RATIO;
            }

            float num2 = MouseOrbitCameraConstants.X_ROTATION_SPEED * deltaTime;
            float num3 = MouseOrbitCameraConstants.Y_ROTATION_SPEED * deltaTime;
            eulerAngles.x -= InputManager.GetUnityAxis(UnityInputConstants.MOUSE_Y) * num2 * num;
            eulerAngles.y += InputManager.GetUnityAxis(UnityInputConstants.MOUSE_X) * num3 * num;
            mouseOrbitCamera.targetRotation = Quaternion.Euler(MouseOrbitCameraUtils.NormalizeEuler(eulerAngles));

            mouseOrbitCamera.distance *=
                Mathf.Pow(3f, 0f - InputManager.GetUnityAxis(UnityInputConstants.MOUSE_SCROLL_WHEEL));

            mouseOrbitCamera.distance = Mathf.Clamp(mouseOrbitCamera.distance,
                MouseOrbitCameraConstants.MIN_DISTANCE,
                MouseOrbitCameraConstants.MAX_DISTANCE);

            Vector3 position = targetNode.cameraTarget.TargetObject.transform.position;

            Vector3 targetPosition =
                position + mouseOrbitCamera.targetRotation * new Vector3(0f, 0f, 0f - mouseOrbitCamera.distance);

            targetPosition =
                MouseOrbitCameraUtils.GetClippedPosition(position, targetPosition, MouseOrbitCameraConstants.MAX_DISTANCE);

            cameraNode.cameraTransformData.Data = new TransformData {
                Position = targetPosition,
                Rotation = mouseOrbitCamera.targetRotation
            };

            ApplyCameraTransformEvent applyCameraTransformEvent = new();
            applyCameraTransformEvent.positionSmoothingRatio = MouseOrbitCameraConstants.POSITION_SMOOTHING_SPEED;
            applyCameraTransformEvent.rotationSmoothingRatio = MouseOrbitCameraConstants.ROTATION_SMOOTHING_SPEED;
            applyCameraTransformEvent.deltaTime = deltaTime;
            ScheduleEvent(applyCameraTransformEvent, cameraNode);
        }

        public class OrbitCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraTransformDataComponent cameraTransformData;
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class CameraTargetNode : Node {
            public CameraTargetComponent cameraTarget;
        }
    }
}