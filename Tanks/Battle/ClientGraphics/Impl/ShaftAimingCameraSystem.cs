using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingCameraSystem : ECSSystem {
        [OnEventFire]
        public void InitManualTargetingCamera(NodeAddedEvent evt, AimingWorkActivationStateNode weapon,
            InitialCameraNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            Entity entity = cameraNode.Entity;
            Camera camera2 = camera.Camera;
            Transform transform = camera2.transform;
            ShaftAimingCameraComponent shaftAimingCameraComponent = new();
            shaftAimingCameraComponent.WorldInitialCameraPosition = transform.position;
            shaftAimingCameraComponent.WorldInitialCameraRotation = transform.rotation;
            shaftAimingCameraComponent.InitialFOV = camera2.fieldOfView;
            shaftAimingCameraComponent.InitialTransform = transform.parent;
            transform.parent = weapon.weaponInstance.WeaponInstance.transform;
            entity.AddComponent(shaftAimingCameraComponent);

            if (entity.HasComponent<ApplyCameraTransformComponent>()) {
                entity.RemoveComponent<ApplyCameraTransformComponent>();
            }

            if (entity.HasComponent<CameraFOVUpdateComponent>()) {
                entity.RemoveComponent<CameraFOVUpdateComponent>();
            }

            if (entity.HasComponent<ShaftAimingCameraFOVRecoveringComponent>()) {
                cameraNode.Entity.RemoveComponent<ShaftAimingCameraFOVRecoveringComponent>();
            }
        }

        [OnEventFire]
        public void InterpolateManualTargetingCamera(UpdateEvent evt, AimingWorkActivationStateNode weapon,
            [JoinAll] AimingCameraNode cameraNode) {
            MuzzleVisualAccessor muzzleVisualAccessor = new(weapon.muzzlePoint);
            CameraComponent camera = cameraNode.camera;
            Camera camera2 = camera.Camera;
            Transform transform = camera2.transform;
            ShaftAimingCameraComponent shaftAimingCamera = cameraNode.shaftAimingCamera;

            float t = Mathf.Clamp01(weapon.shaftAimingWorkActivationState.ActivationTimer /
                                    weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec);

            Vector3 worldInitialCameraPosition = shaftAimingCamera.WorldInitialCameraPosition;

            Vector3 barrelOriginWorld =
                new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();

            transform.position = Vector3.Lerp(worldInitialCameraPosition, barrelOriginWorld, t);
            Quaternion worldInitialCameraRotation = shaftAimingCamera.WorldInitialCameraRotation;

            Quaternion quaternion = Quaternion.LookRotation(muzzleVisualAccessor.GetFireDirectionWorld(),
                muzzleVisualAccessor.GetUpDirectionWorld());

            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = Mathf.Clamp(
                weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle,
                0f - weapon.shaftAimingRotationConfig.AimingOffsetClipping,
                weapon.shaftAimingRotationConfig.AimingOffsetClipping);

            Vector3 eulerAngles = quaternion.eulerAngles;

            quaternion = Quaternion.Euler(eulerAngles.x,
                eulerAngles.y + weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle,
                eulerAngles.z);

            transform.rotation = Quaternion.Slerp(worldInitialCameraRotation, quaternion, t);

            camera2.fieldOfView = Mathf.Lerp(shaftAimingCamera.InitialFOV,
                weapon.shaftAimingCameraConfigEffect.ActivationStateTargetFov,
                t);
        }

        [OnEventFire]
        public void UpdateCameraAtWorkingState(WeaponRotationUpdateShaftAimingCameraEvent e, AimingWorkingStateNode weapon,
            [JoinAll] AimingCameraNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            Camera camera2 = camera.Camera;
            Transform transform = camera2.transform;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect = weapon.shaftAimingCameraConfigEffect;
            ShaftAimingWorkingStateComponent shaftAimingWorkingState = weapon.shaftAimingWorkingState;

            transform.rotation = Quaternion.LookRotation(shaftAimingWorkingState.WorkingDirection,
                muzzleLogicAccessor.GetUpDirectionWorld());

            Vector3 localEulerAngles = transform.localEulerAngles;

            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = Mathf.Clamp(
                weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle,
                0f - weapon.shaftAimingRotationConfig.AimingOffsetClipping,
                weapon.shaftAimingRotationConfig.AimingOffsetClipping);

            float num = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle,
                0f - weapon.shaftAimingRotationConfig.MaxAimingCameraOffset,
                weapon.shaftAimingRotationConfig.MaxAimingCameraOffset);

            localEulerAngles.y += num;
            localEulerAngles.x -= weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle;
            transform.localEulerAngles = localEulerAngles;
            float activationStateTargetFov = shaftAimingCameraConfigEffect.ActivationStateTargetFov;
            float workingStateMinFov = shaftAimingCameraConfigEffect.WorkingStateMinFov;
            float t = shaftAimingWorkingState.InitialEnergy - weapon.weaponEnergy.Energy;
            camera2.fieldOfView = Mathf.Lerp(activationStateTargetFov, workingStateMinFov, t);
        }

        [OnEventFire]
        public void ResetCamera(NodeAddedEvent evt, AimingIdleStateNode weapon, AimingCameraNode cameraNode) {
            ShaftAimingCameraComponent shaftAimingCamera = cameraNode.shaftAimingCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            CameraComponent camera = cameraNode.camera;
            Camera camera2 = camera.Camera;
            Transform transform = camera2.transform;
            transform.position = cameraTransformData.Data.Position;
            transform.rotation = cameraTransformData.Data.Rotation;
            transform.parent = shaftAimingCamera.InitialTransform;
            Entity entity = cameraNode.Entity;
            entity.RemoveComponent<ShaftAimingCameraComponent>();
            entity.AddComponent<ShaftAimingCameraFOVRecoveringComponent>();
            entity.AddComponent<ApplyCameraTransformComponent>();
        }

        [OnEventFire]
        public void RecoverFOV(UpdateEvent evt, ShaftAimingCameraEffectConfigNode weapon,
            [JoinAll] CameraRecoveringNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            BattleCameraComponent battleCamera = cameraNode.battleCamera;
            Camera camera2 = camera.Camera;
            ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect = weapon.shaftAimingCameraConfigEffect;
            float recoveringFovSpeed = shaftAimingCameraConfigEffect.RecoveringFovSpeed;
            float optimalFOV = battleCamera.OptimalFOV;
            camera2.fieldOfView += recoveringFovSpeed * evt.DeltaTime;

            if (camera2.fieldOfView >= optimalFOV) {
                camera2.fieldOfView = optimalFOV;
                Entity entity = cameraNode.Entity;
                entity.RemoveComponent<ShaftAimingCameraFOVRecoveringComponent>();
                entity.AddComponent<CameraFOVUpdateComponent>();
            }
        }

        public class ShaftAimingCameraEffectConfigNode : Node {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;

            public ShaftStateControllerComponent shaftStateController;
        }

        public class AimingWorkActivationStateNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;

            public ShaftAimingRotationConfigComponent shaftAimingRotationConfig;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftStateConfigComponent shaftStateConfig;

            public ShaftStateControllerComponent shaftStateController;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class AimingIdleStateNode : Node {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;
            public ShaftIdleStateComponent shaftIdleState;

            public ShaftStateControllerComponent shaftStateController;

            public WeaponInstanceComponent weaponInstance;
        }

        public class AimingWorkingStateNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;

            public ShaftAimingRotationConfigComponent shaftAimingRotationConfig;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftStateControllerComponent shaftStateController;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class InitialCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }

        [Not(typeof(CameraFOVUpdateComponent))]
        public class CameraRecoveringNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;

            public ShaftAimingCameraFOVRecoveringComponent shaftAimingCameraFOVRecovering;
        }

        [Not(typeof(ApplyCameraTransformComponent))]
        public class AimingCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;

            public CameraTransformDataComponent cameraTransformData;

            public ShaftAimingCameraComponent shaftAimingCamera;
        }
    }
}