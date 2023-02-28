using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FreeCameraSystem : ECSSystem {
        static readonly Vector3[] clipDirections = new Vector3[6] {
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.down,
            Vector3.forward,
            Vector3.back
        };

        static readonly float CLIP_DISTANCE = 2f;

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, FreeCameraNode cameraNode) {
            FreeCameraComponent freeCamera = cameraNode.freeCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            freeCamera.Data = cameraTransformData.Data;
        }

        [OnEventFire]
        public void RecieveInput(TimeUpdateEvent evt, FreeCameraNode cameraNode, [JoinAll] UserNode user) {
            float deltaTime = evt.DeltaTime;
            FreeCameraComponent freeCamera = cameraNode.freeCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            Quaternion rotation = freeCamera.Data.Rotation;
            Vector3 position = freeCamera.Data.Position;
            bool flag = freeCamera.Data != cameraTransformData.Data;
            float num = 1f;
            float num2 = 1f;

            if (InputManager.CheckAction(SpectatorCameraActions.AccelerateMovement)) {
                num2 *= freeCamera.speedUp;
            }

            if (InputManager.CheckAction(SpectatorCameraActions.SlowMovement)) {
                num *= freeCamera.slowDown;
                num2 *= freeCamera.slowDown;
            }

            if (InputManager.GetActionKeyDown(FreeCameraActions.FIXED_HEIGHT)) {
                freeCamera.fixedHeight ^= true;
            }

            float num3 = ((InputManager.CheckAction(FreeCameraActions.UP_TURN) ? -1 : 0) + (InputManager.CheckAction(FreeCameraActions.DOWN_TURN) ? 1 : 0)) * 0.5f;
            float num4 = ((InputManager.CheckAction(FreeCameraActions.RIGHT_TURN) ? 1 : 0) + (InputManager.CheckAction(FreeCameraActions.LEFT_TURN) ? -1 : 0)) * 0.5f;

            if (InputManager.CheckMouseButtonInAllActiveContexts(FreeCameraActions.MOUSE_BUTTON_DOWN, UnityInputConstants.MOUSE_BUTTON_LEFT)) {
                num3 += 0f - InputManager.GetUnityAxis(UnityInputConstants.MOUSE_Y);
                num4 += InputManager.GetUnityAxis(UnityInputConstants.MOUSE_X);
            }

            Vector3 eulerAngles = rotation.eulerAngles;
            eulerAngles.x += num3 * freeCamera.xRotationSpeed * num * deltaTime;
            eulerAngles.y += num4 * freeCamera.yRotationSpeed * num * deltaTime;
            eulerAngles = NormalizeEuler(eulerAngles, freeCamera.xMinAngle, freeCamera.xMaxAngle);
            rotation = Quaternion.Euler(eulerAngles);
            float num5 = (InputManager.CheckAction(FreeCameraActions.FORWARD_MOVING) ? 1 : 0) + (InputManager.CheckAction(FreeCameraActions.BACK_MOVING) ? -1 : 0);
            float num6 = (InputManager.CheckAction(FreeCameraActions.RIGHT_MOVING) ? 1 : 0) + (InputManager.CheckAction(FreeCameraActions.LEFT_MOVING) ? -1 : 0);
            float num7 = InputManager.GetAxis(FreeCameraActions.MOUSE_SCROLL_WHEEL_UP, true) - InputManager.GetAxis(FreeCameraActions.MOUSE_SCROLL_WHEEL_DOWN, true);
            num7 += InputManager.GetAxis(FreeCameraActions.UP_MOVING) - InputManager.GetAxis(FreeCameraActions.DOWN_MOVING);
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;
            Vector3 vector = Vector3.up;

            if (freeCamera.fixedHeight) {
                Vector3 eulerAngles2 = rotation.eulerAngles;
                eulerAngles2.x = 0f;
                Quaternion quaternion = Quaternion.Euler(eulerAngles2);
                forward = quaternion * forward;
                right = quaternion * right;
            } else {
                forward = rotation * forward;
                right = rotation * right;
                vector = rotation * vector;
            }

            Vector3 vector2 = Vector3.ClampMagnitude(forward * num5 + right * num6 + vector * num7, 1f);
            Vector3 targetPosition = position + vector2 * freeCamera.flySpeed * num2 * deltaTime;
            position = GetClippedPosition(position, targetPosition);
            TransformData transformData = default;
            transformData.Position = position;
            transformData.Rotation = rotation;
            TransformData transformData2 = transformData;
            ApplyCameraTransformEvent applyCameraTransformEvent = ApplyCameraTransformEvent.ResetApplyCameraTransformEvent();

            if (transformData2 != freeCamera.Data) {
                cameraNode.cameraTransformData.Data = transformData2;
                freeCamera.Data = transformData2;
                applyCameraTransformEvent.PositionSmoothingRatio = freeCamera.positionSmoothingSpeed;
                applyCameraTransformEvent.RotationSmoothingRatio = freeCamera.rotationSmoothingSpeed;
            }

            applyCameraTransformEvent.DeltaTime = deltaTime;
            ScheduleEvent(applyCameraTransformEvent, cameraNode);
        }

        static Vector3 NormalizeEuler(Vector3 euler, float xMinAngle, float xMaxAngle) {
            euler.x = Mathf.Clamp((euler.x + 180f) % 360f - 180f, xMinAngle, xMaxAngle);
            euler.y = (euler.y + 360f) % 360f;
            euler.z = 0f;
            return euler;
        }

        static Vector3 GetClippedPosition(Vector3 currentPosition, Vector3 targetPosition) {
            RaycastHit hitInfo;

            for (int i = 0; i < clipDirections.Length; i++) {
                Vector3 vector = targetPosition - currentPosition;

                if (Vector3.Dot(vector, clipDirections[i]) > 0f &&
                    Physics.Linecast(currentPosition - clipDirections[i], currentPosition + clipDirections[i] * CLIP_DISTANCE, out hitInfo, LayerMasks.STATIC)) {
                    targetPosition -= hitInfo.normal * Vector3.Dot(hitInfo.normal, vector);
                }
            }

            if (Physics.Linecast(targetPosition + Vector3.up, targetPosition + Vector3.down * CLIP_DISTANCE, out hitInfo, LayerMasks.STATIC)) {
                targetPosition.y = hitInfo.point.y + CLIP_DISTANCE;
            }

            return targetPosition;
        }

        public class FreeCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraTransformDataComponent cameraTransformData;
            public FreeCameraComponent freeCamera;
        }

        public class UserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}