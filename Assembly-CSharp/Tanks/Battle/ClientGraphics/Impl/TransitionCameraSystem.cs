using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TransitionCameraSystem : ECSSystem {
        const float EPS_DIST = 0.01f;

        static readonly float SPECTATOR_TRANSITION_CAMERA_POSITION_SMOOTHING_RATIO = 10f;

        static readonly float SPECTATOR_TRANSITION_CAMERA_ROTATION_SMOOTHING_RATIO = 12f;

        [OnEventFire]
        public void InitCamera(NodeAddedEvent evt, TransitionCameraNode transitionCameraNode, TankNode tank, [JoinByTank] [Context] SpawnTankNode spawnTank,
            [Context] [JoinByTank] WeaponNode weapon) {
            SpawnCameraConfigUnityComponent spawnCameraConfigUnity = transitionCameraNode.spawnCameraConfigUnity;
            InitFollowCamera(transitionCameraNode, weapon, tank, spawnCameraConfigUnity.FlyTimeSec, spawnCameraConfigUnity.FlyHeight);
        }

        [OnEventFire]
        public void InitCamera(NodeAddedEvent evt, SpectatorTransitionCameraNode transitionCameraNode, TankNode tank, [Context] [JoinByTank] WeaponNode weapon,
            [JoinByUser] FollowedUserNode followedUser) {
            float num = 0.55f;
            float num2 = 1f;

            if (transitionCameraNode.transitionCamera.Spawn) {
                SpawnCameraConfigUnityComponent spawnCameraConfigUnity = transitionCameraNode.spawnCameraConfigUnity;
                num = spawnCameraConfigUnity.FlyTimeSec;
                num2 = spawnCameraConfigUnity.FlyHeight;
            } else {
                TransitionCameraConfigUnityComponent transitionCameraConfigUnity = transitionCameraNode.transitionCameraConfigUnity;
                num = transitionCameraConfigUnity.FlyTimeSec;
                num2 = transitionCameraConfigUnity.FlyHeight;
            }

            switch (transitionCameraNode.transitionCamera.CameraSaveData.Type) {
                case CameraType.Follow:
                    InitFollowCamera(transitionCameraNode, weapon, tank, num, num2);
                    break;

                case CameraType.MouseOrbit: {
                    TransformData targetMouseOrbitCameraTransformData = MouseOrbitCameraUtils.GetTargetMouseOrbitCameraTransformData(weapon.cameraTarget.TargetObject.transform,
                        transitionCameraNode.transitionCamera.CameraSaveData.MouseOrbitDistance, transitionCameraNode.transitionCamera.CameraSaveData.MouseOrbitTargetRotation);

                    InitCamera(transitionCameraNode, targetMouseOrbitCameraTransformData.Position, targetMouseOrbitCameraTransformData.Rotation.eulerAngles, num, num2);
                    break;
                }
            }
        }

        void InitFollowCamera(TransitionCameraNode cameraNode, WeaponNode weapon, TankNode tank, float flyTimeSec, float flyHeight) {
            TransitionCameraComponent transitionCamera = cameraNode.transitionCamera;
            Transform transform = weapon.cameraTarget.TargetObject.transform;
            BezierPosition bezierPosition = cameraNode.bezierPosition.BezierPosition;
            CameraOffsetConfigComponent cameraOffsetConfig = cameraNode.cameraOffsetConfig;
            Vector3 cameraOffset = new(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset);
            CameraData cameraData = new();

            Vector3 position = CameraPositionCalculator.CalculateCameraPosition(transform, tank.baseRenderer, tank.tankColliders.BoundsCollider.bounds.center, bezierPosition, cameraData,
                cameraOffset, 0f);

            Vector3 zero = Vector3.zero;
            zero.x = (0f - CameraPositionCalculator.GetPitchAngle(cameraData, bezierPosition)) * 57.29578f;
            zero.y = transform.transform.rotation.eulerAngles.y;
            InitCamera(cameraNode, position, zero, flyTimeSec, flyHeight);
        }

        [OnEventFire]
        public void InitCamera(NodeAddedEvent evt, SpectatorTransitionCameraNode transitionCameraNode) {
            TransitionCameraConfigUnityComponent transitionCameraConfigUnity = transitionCameraNode.transitionCameraConfigUnity;
            CameraType type = transitionCameraNode.transitionCamera.CameraSaveData.Type;

            if (type == CameraType.Free) {
                TransformData transformData = transitionCameraNode.transitionCamera.CameraSaveData.TransformData;
                Vector3 position = transformData.Position;
                Vector3 eulerAngles = transformData.Rotation.eulerAngles;
                InitCamera(transitionCameraNode, position, eulerAngles, transitionCameraConfigUnity.FlyTimeSec, transitionCameraConfigUnity.FlyHeight);
            }
        }

        static void InitCamera(TransitionCameraNode transitionCameraNode, Vector3 position, Vector3 angles, float flyTimeSec, float flyHeight) {
            TransitionCameraComponent transitionCamera = transitionCameraNode.transitionCamera;
            CameraTransformDataComponent cameraTransformData = transitionCameraNode.cameraTransformData;
            transitionCamera.P1 = cameraTransformData.Data.Position;
            transitionCamera.P2 = transitionCamera.P1;
            transitionCamera.P4 = position;
            transitionCamera.P3 = transitionCamera.P4;
            float y = (!(transitionCamera.P1.y > transitionCamera.P4.y) ? transitionCamera.P4.y : transitionCamera.P1.y) + flyHeight;
            transitionCamera.P2 = new Vector3(transitionCamera.P2.x, y, transitionCamera.P2.z);
            transitionCamera.P3 = new Vector3(transitionCamera.P3.x, y, transitionCamera.P3.z);
            float num = 4f / (flyTimeSec * flyTimeSec);
            Vector3 eulerAngles = cameraTransformData.Data.Rotation.eulerAngles;
            transitionCamera.AngleValuesX = new AngleValues(eulerAngles.x, angles.x, num);
            transitionCamera.AngleValuesY = new AngleValues(eulerAngles.y, angles.y, num);
            transitionCamera.TotalDistance = (transitionCamera.P4 - transitionCamera.P1).magnitude;
            transitionCamera.Acceleration = transitionCamera.TotalDistance * num;
        }

        [OnEventFire]
        public void UpdateCamera(TimeUpdateEvent e, TransitionCameraNode transitionCameraNode, [JoinAll] SingleNode<SelfTankComponent> selfTank) {
            UpdateCamera(e.DeltaTime, transitionCameraNode);
            ScheduleEvent(ApplyCameraTransformEvent.ResetApplyCameraTransformEvent(), transitionCameraNode);

            if (transitionCameraNode.transitionCamera.TransitionComplete) {
                transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraFollowState>();
            }
        }

        [OnEventFire]
        public void UpdateCamera(TimeUpdateEvent e, SpectatorTransitionCameraNode transitionCameraNode, [JoinAll] Optional<SingleNode<CameraTargetComponent>> target) {
            CameraSaveData cameraSaveData = transitionCameraNode.transitionCamera.CameraSaveData;

            if (cameraSaveData.Type != CameraType.Free && !target.IsPresent()) {
                return;
            }

            UpdateCamera(e.DeltaTime, transitionCameraNode);
            ApplyCameraTransformEvent applyCameraTransformEvent = ApplyCameraTransformEvent.ResetApplyCameraTransformEvent();
            applyCameraTransformEvent.PositionSmoothingRatio = SPECTATOR_TRANSITION_CAMERA_POSITION_SMOOTHING_RATIO;
            applyCameraTransformEvent.RotationSmoothingRatio = SPECTATOR_TRANSITION_CAMERA_ROTATION_SMOOTHING_RATIO;
            applyCameraTransformEvent.DeltaTime = e.DeltaTime;
            ScheduleEvent(applyCameraTransformEvent, transitionCameraNode);

            if (transitionCameraNode.transitionCamera.TransitionComplete) {
                switch (cameraSaveData.Type) {
                    case CameraType.Follow:
                        transitionCameraNode.bezierPosition.BezierPosition.SetBaseRatio(cameraSaveData.FollowCameraBezierPositionRatio);
                        transitionCameraNode.bezierPosition.BezierPosition.SetRatioOffset(cameraSaveData.FollowCameraBezierPositionRatioOffset);
                        transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraFollowState>();
                        break;

                    case CameraType.MouseOrbit: {
                        MouseOrbitCameraComponent mouseOrbitCamera = transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraOrbitState>().mouseOrbitCamera;
                        mouseOrbitCamera.distance = cameraSaveData.MouseOrbitDistance;
                        mouseOrbitCamera.targetRotation = cameraSaveData.MouseOrbitTargetRotation;
                        break;
                    }

                    case CameraType.Free:
                        transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraFreeState>();
                        break;
                }
            }
        }

        static void UpdateCamera(float dt, TransitionCameraNode cameraNode) {
            TransitionCameraComponent transitionCamera = cameraNode.transitionCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;

            if (transitionCamera.TotalDistance == 0f) {
                transitionCamera.TransitionComplete = true;
                return;
            }

            if (transitionCamera.Speed < 0f) {
                transitionCamera.TransitionComplete = true;
                return;
            }

            if (transitionCamera.Distance > 0.5f * transitionCamera.TotalDistance && transitionCamera.Acceleration > 0f) {
                transitionCamera.Acceleration = 0f - transitionCamera.Acceleration;
                transitionCamera.AngleValuesX.ReverseAcceleration();
                transitionCamera.AngleValuesY.ReverseAcceleration();
            }

            float num = transitionCamera.Acceleration * dt;
            transitionCamera.Distance += (transitionCamera.Speed + 0.5f * num) * dt;
            transitionCamera.Speed += num;

            if (transitionCamera.Distance > transitionCamera.TotalDistance) {
                transitionCamera.Distance = transitionCamera.TotalDistance;
            }

            Vector3 position = Bezier.PointOnCurve(transitionCamera.Distance / transitionCamera.TotalDistance, transitionCamera.P1, transitionCamera.P2, transitionCamera.P3,
                transitionCamera.P4);

            Vector3 eulerAngles = cameraTransformData.Data.Rotation.eulerAngles;
            Vector3 euler = eulerAngles + new Vector3(transitionCamera.AngleValuesX.Update(dt), transitionCamera.AngleValuesY.Update(dt), 0f);

            cameraTransformData.Data = new TransformData {
                Position = position,
                Rotation = Quaternion.Euler(euler)
            };

            if (Mathf.Abs(transitionCamera.Distance - transitionCamera.TotalDistance) < 0.01f) {
                transitionCamera.TransitionComplete = true;
            }
        }

        public class TransitionCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public BezierPositionComponent bezierPosition;

            public CameraESMComponent cameraESM;

            public CameraOffsetConfigComponent cameraOffsetConfig;

            public CameraTransformDataComponent cameraTransformData;

            public SpawnCameraConfigUnityComponent spawnCameraConfigUnity;
            public TransitionCameraComponent transitionCamera;

            public TransitionCameraConfigUnityComponent transitionCameraConfigUnity;

            public TransitionCameraStateComponent transitionCameraState;
        }

        public class SpectatorTransitionCameraNode : TransitionCameraNode {
            public SpectatorCameraComponent spectatorCamera;
        }

        public class WeaponNode : Node {
            public CameraTargetComponent cameraTarget;

            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
        }

        public class TankNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankComponent tank;

            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class SpawnTankNode : Node {
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;

            public TankSpawnStateComponent tankSpawnState;
        }

        public class WeaponInstanceNode : Node {
            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
            public WeaponInstanceComponent weaponInstance;
        }

        public class FollowedUserNode : Node {
            public FollowedBattleUserComponent followedBattleUser;

            public UserGroupComponent userGroup;
        }
    }
}