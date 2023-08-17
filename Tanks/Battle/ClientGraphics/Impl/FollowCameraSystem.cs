using System;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FollowCameraSystem : ECSSystem {
        static readonly float SPECTATOR_FOLLOW_CAMERA_POSITION_SMOOTHING_RATIO = 10f;

        static readonly float SPECTATOR_FOLLOW_CAMERA_ROTATION_RATIO = 12f;

        public static float BEZIER_SPEED_BY_MOUSE_VERTICAL = 6f;

        public static float MOUSE_WHEEL_RATIO = 0.032f;

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitFollowCameraData(NodeAddedEvent evt, FollowCameraNode cameraNode) =>
            cameraNode.followCamera.cameraData = new CameraData();

        [OnEventFire]
        public void Follow(CameraFollowEvent e, CameraTargetNode cameraTargetNode, [JoinByTank] TankNode tank,
            [JoinAll] CameraNode cameraNode,
            [JoinAll] Optional<SingleNode<FollowCameraComponent>> followCameraOptionalNode) {
            Transform transform = cameraTargetNode.cameraTarget.TargetObject.transform;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            CameraOffsetConfigComponent cameraOffsetConfig = cameraNode.cameraOffsetConfig;
            Vector3 cameraOffset = new(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset);
            BezierPosition bezierPosition = cameraNode.bezierPosition.BezierPosition;

            cameraTransformData.Data = CameraPositionCalculator.GetTargetFollowCameraTransformData(transform,
                tank.baseRenderer,
                tank.tankColliders.BoundsCollider.bounds.center,
                bezierPosition,
                cameraOffset);

            cameraNode.camera.Camera.transform.position = cameraTransformData.Data.Position;
            cameraNode.camera.Camera.transform.rotation = cameraTransformData.Data.Rotation;
        }

        [OnEventComplete]
        public void Follow(CameraFollowEvent e, SingleNode<WeaponInstanceComponent> weapon,
            [JoinAll] SmoothingCameraNode battleCamera) {
            battleCamera.transformTimeSmoothingData.LastPosition = battleCamera.camera.Camera.transform.position;
            battleCamera.transformTimeSmoothingData.LastRotation = battleCamera.camera.Camera.transform.rotation;
        }

        [OnEventComplete]
        public void UpdateFollowCameraData(UpdateEvent e, FollowCameraNode cameraNode, [JoinAll] CameraTargetNode targetNode,
            [JoinByTank] TankNode tank, [JoinAll] SingleNode<SelfTankComponent> selfTank,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolderNode) {
            UpdateFollowCameraData(e.DeltaTime, cameraNode, targetNode, tank, mouseControlStateHolderNode.component);
            ScheduleEvent<ApplyCameraTransformEvent>(cameraNode);
        }

        [OnEventFire]
        public void UpdateFollowCameraData(UpdateEvent e, FollowSpectratorCameraNode cameraNode,
            [JoinAll] CameraTargetNode targetNode, [JoinByTank] TankNode tank,
            [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolderNode) {
            UpdateFollowCameraData(e.DeltaTime, cameraNode, targetNode, tank, mouseControlStateHolderNode.component);
            ApplyCameraTransformEvent applyCameraTransformEvent = new();
            applyCameraTransformEvent.positionSmoothingRatio = SPECTATOR_FOLLOW_CAMERA_POSITION_SMOOTHING_RATIO;
            applyCameraTransformEvent.rotationSmoothingRatio = SPECTATOR_FOLLOW_CAMERA_ROTATION_RATIO;
            applyCameraTransformEvent.deltaTime = e.DeltaTime;
            ScheduleEvent(applyCameraTransformEvent, cameraNode);
        }

        void UpdateFollowCameraData(float deltaTime, FollowCameraNode cameraNode, CameraTargetNode targetNode, TankNode tank,
            MouseControlStateHolderComponent mouseControlStateHolder) {
            CameraTargetComponent cameraTarget = targetNode.cameraTarget;
            GameObject targetObject = cameraTarget.TargetObject;

            if (!(targetObject == null)) {
                CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
                Transform transform = targetObject.transform;
                FollowCameraComponent followCamera = cameraNode.followCamera;
                BezierPosition bezierPosition = cameraNode.bezierPosition.BezierPosition;
                CameraData cameraData = followCamera.cameraData;

                UpdateBezierPosition(bezierPosition,
                    followCamera.verticalCameraSpeed,
                    deltaTime,
                    targetNode.weaponRotationControl.MouseRotationCumulativeVerticalAngle,
                    mouseControlStateHolder.MouseVerticalInverted);

                targetNode.weaponRotationControl.MouseRotationCumulativeVerticalAngle = 0f;
                float num = targetNode.weaponRotationControl.MouseRotationCumulativeHorizontalAngle;
                float degrees = Vector3.Angle(transform.up, Vector3.up);
                degrees = MathUtil.ClampAngle180(degrees);

                if (Mathf.Abs(degrees) > 90f) {
                    num *= -1f;
                }

                bool mouse = targetNode.weaponRotationControl.MouseRotationCumulativeHorizontalAngle != 0f;
                CameraOffsetConfigComponent cameraOffsetConfig = cameraNode.cameraOffsetConfig;

                Vector3 cameraCalculated = CameraPositionCalculator.CalculateCameraPosition(
                    cameraOffset:
                    new Vector3(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset),
                    target: transform,
                    tankRendererComponent: tank.baseRenderer,
                    tankBoundsCenter: tank.tankColliders.BoundsCollider.bounds.center,
                    bezierPosition: bezierPosition,
                    cameraData: cameraData,
                    additionalAngle: num);

                Vector3 position = cameraTransformData.Data.Position;
                Vector3 rotation = cameraTransformData.Data.Rotation.eulerAngles;
                rotation *= (float)Math.PI / 180f;
                CameraPositionCalculator.CalculatePitchMovement(ref rotation, bezierPosition, deltaTime, cameraData, mouse);
                Vector3 targetDirection = Quaternion.Euler(new Vector3(0f, num, 0f)) * transform.forward;
                CameraPositionCalculator.CalculateYawMovement(targetDirection, ref rotation, deltaTime, cameraData, mouse);
                CameraPositionCalculator.SmoothReturnRoll(ref rotation, followCamera.rollReturnSpeedDegPerSec, deltaTime);

                cameraTransformData.Data = new TransformData {
                    Position = CameraPositionCalculator.CalculateLinearMovement(deltaTime,
                        cameraCalculated,
                        position,
                        cameraData,
                        transform,
                        mouse),
                    Rotation = Quaternion.Euler(rotation * 57.29578f)
                };
            }
        }

        void UpdateBezierPosition(BezierPosition bezierPosition, float verticalCameraSpeed, float dt, float vertical,
            bool mouseInvert) {
            float axis = InputManager.GetAxis(CameraRotationActions.ROTATE_CAMERA_UP_LEFT);
            float axis2 = InputManager.GetAxis(CameraRotationActions.ROTATE_CAMERA_DOWN_RIGHT);
            float num = InputManager.GetAxis(CameraRotationActions.MOUSEWHEEL_MOVE) * MOUSE_WHEEL_RATIO;

            if (mouseInvert) {
                num *= -1f;
            }

            float num2 = axis - axis2;
            float num3 = num2 * verticalCameraSpeed * dt + num;
            bezierPosition.SetBaseRatio(bezierPosition.GetBaseRatio() + num3);
            float num4 = BEZIER_SPEED_BY_MOUSE_VERTICAL / 180f;
            bezierPosition.SetRatioOffset(bezierPosition.GetRatioOffset() + vertical * num4);
        }

        float GetHeightChangeDir(bool mouseInvert) {
            float axis = InputManager.GetAxis(CameraRotationActions.ROTATE_CAMERA_UP_LEFT);
            float axis2 = InputManager.GetAxis(CameraRotationActions.ROTATE_CAMERA_DOWN_RIGHT);
            float num = InputManager.GetAxis(CameraRotationActions.MOUSEWHEEL_MOVE) * MOUSE_WHEEL_RATIO;

            if (mouseInvert) {
                num *= -1f;
            }

            return axis - axis2 + num;
        }

        public class CameraTargetNode : Node {
            public CameraTargetComponent cameraTarget;

            public TankGroupComponent tankGroup;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class TankNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankComponent tank;

            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public BezierPositionComponent bezierPosition;

            public CameraComponent camera;

            public CameraESMComponent cameraEsm;

            public CameraOffsetConfigComponent cameraOffsetConfig;

            public CameraTransformDataComponent cameraTransformData;
        }

        public class FreeCameraNode : CameraNode {
            public FreeCameraComponent freeCamera;
        }

        public class FollowCameraNode : CameraNode {
            public FollowCameraComponent followCamera;
        }

        public class FollowSpectratorCameraNode : FollowCameraNode {
            public SpectatorCameraComponent spectatorCamera;
        }

        public class MouseOrbitCameraNode : CameraNode {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class UserWeaponNode : Node {
            public UserGroupComponent userGroup;

            public WeaponInstanceComponent weaponInstance;
        }

        public class BattleCameraNode : Node {
            public ApplyCameraTransformComponent applyCameraTransform;

            public BattleCameraComponent battleCamera;
            public CameraComponent camera;

            public CameraTransformDataComponent cameraTransformData;
        }

        public class SmoothingCameraNode : BattleCameraNode {
            public TransformTimeSmoothingDataComponent transformTimeSmoothingData;
        }
    }
}