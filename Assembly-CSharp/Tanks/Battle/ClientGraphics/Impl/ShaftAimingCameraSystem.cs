using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingCameraSystem : ECSSystem {
        [OnEventFire]
        public void InitManualTargetingCamera(NodeAddedEvent evt, AimingWorkActivationStateNode weapon, InitialCameraNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            Entity entity = cameraNode.Entity;
            Transform root = cameraNode.cameraRootTransform.Root;
            ShaftAimingCameraComponent shaftAimingCameraComponent = new();
            shaftAimingCameraComponent.WorldInitialCameraPosition = root.position;
            shaftAimingCameraComponent.WorldInitialCameraRotation = root.rotation;
            shaftAimingCameraComponent.InitialFOV = camera.FOV;
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
        public void InterpolateManualTargetingCamera(UpdateEvent evt, AimingWorkActivationStateNode weapon, [JoinAll] AimingCameraNode cameraNode) {
            MuzzleVisualAccessor muzzleVisualAccessor = new(weapon.muzzlePoint);
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            ShaftAimingCameraComponent shaftAimingCamera = cameraNode.shaftAimingCamera;
            float t = Mathf.Clamp01(weapon.shaftAimingWorkActivationState.ActivationTimer / weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec);
            Vector3 worldInitialCameraPosition = shaftAimingCamera.WorldInitialCameraPosition;
            Vector3 barrelOriginWorld = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();
            root.SetPositionSafe(Vector3.Lerp(worldInitialCameraPosition, barrelOriginWorld, t));
            Quaternion worldInitialCameraRotation = shaftAimingCamera.WorldInitialCameraRotation;
            Quaternion quaternion = Quaternion.LookRotation(muzzleVisualAccessor.GetFireDirectionWorld(), muzzleVisualAccessor.GetUpDirectionWorld());

            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle,
                0f - weapon.shaftAimingRotationConfig.AimingOffsetClipping, weapon.shaftAimingRotationConfig.AimingOffsetClipping);

            Vector3 eulerAngles = quaternion.eulerAngles;
            quaternion = Quaternion.Euler(eulerAngles.x, eulerAngles.y + weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle, eulerAngles.z);
            root.SetRotationSafe(Quaternion.Slerp(worldInitialCameraRotation, quaternion, t));
            camera.FOV = Mathf.Lerp(shaftAimingCamera.InitialFOV, weapon.shaftAimingCameraConfigEffect.ActivationStateTargetFov, t);
        }

        [OnEventFire]
        public void UpdateCameraAtWorkingState(WeaponRotationUpdateShaftAimingCameraEvent e, AimingWorkingStateNode weapon, [JoinAll] AimingCameraNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect = weapon.shaftAimingCameraConfigEffect;
            ShaftAimingWorkingStateComponent shaftAimingWorkingState = weapon.shaftAimingWorkingState;
            root.SetPositionSafe(muzzleLogicAccessor.GetBarrelOriginWorld());
            root.SetRotationSafe(Quaternion.LookRotation(shaftAimingWorkingState.WorkingDirection, muzzleLogicAccessor.GetUpDirectionWorld()));
            Vector3 eulerAngles = root.eulerAngles;

            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle,
                0f - weapon.shaftAimingRotationConfig.AimingOffsetClipping, weapon.shaftAimingRotationConfig.AimingOffsetClipping);

            float num = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle, 0f - weapon.shaftAimingRotationConfig.MaxAimingCameraOffset,
                weapon.shaftAimingRotationConfig.MaxAimingCameraOffset);

            eulerAngles.y += num;
            eulerAngles.x -= weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle;
            root.eulerAngles = eulerAngles;
            float activationStateTargetFov = shaftAimingCameraConfigEffect.ActivationStateTargetFov;
            float workingStateMinFov = shaftAimingCameraConfigEffect.WorkingStateMinFov;
            float t = shaftAimingWorkingState.InitialEnergy - weapon.weaponEnergy.Energy;
            camera.FOV = Mathf.Lerp(activationStateTargetFov, workingStateMinFov, t);
        }

        [OnEventFire]
        public void SetKickBackEffectForCameraOnAimingShot(ShaftAimingShotPrepareEvent evt, ShaftWeaponNode shaft) {
            shaft.Entity.AddComponent(new ShaftAimingCameraKickbackComponent(shaft.weaponInstance.WeaponInstance.transform.position,
                shaft.weaponInstance.WeaponInstance.transform.rotation));
        }

        [OnEventFire]
        public void ApplyKickbackEffect(UpdateEvent evt, ShaftWeaponKickbackNode shaft, [JoinAll] AimingCameraNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            Vector3 lastPosition = shaft.shaftAimingCameraKickback.LastPosition;
            Quaternion lastRotation = shaft.shaftAimingCameraKickback.LastRotation;
            Vector3 position = shaft.weaponInstance.WeaponInstance.transform.position;
            Quaternion rotation = shaft.weaponInstance.WeaponInstance.transform.rotation;
            shaft.shaftAimingCameraKickback.LastRotation = rotation;
            shaft.shaftAimingCameraKickback.LastPosition = position;
            root.SetPositionSafe(root.position + position - lastPosition);
            root.SetRotationSafe(root.rotation * (Quaternion.Inverse(lastRotation) * rotation));
        }

        [OnEventFire]
        public void ResetKickBackEffect(NodeAddedEvent evt, AimingIdleKickbackStateNode shaft) {
            shaft.Entity.RemoveComponent<ShaftAimingCameraKickbackComponent>();
        }

        [OnEventFire]
        public void ResetCamera(NodeAddedEvent evt, AimingIdleStateNode weapon, AimingCameraNode cameraNode) {
            ShaftAimingCameraComponent shaftAimingCamera = cameraNode.shaftAimingCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            root.SetPositionSafe(cameraTransformData.Data.Position);
            root.SetRotationSafe(cameraTransformData.Data.Rotation);
            Entity entity = cameraNode.Entity;
            entity.RemoveComponent<ShaftAimingCameraComponent>();
            entity.AddComponent<ShaftAimingCameraFOVRecoveringComponent>();
            entity.AddComponent<ApplyCameraTransformComponent>();
        }

        [OnEventFire]
        public void RecoverFOV(UpdateEvent evt, ShaftAimingCameraEffectConfigNode weapon, [JoinAll] CameraRecoveringNode cameraNode) {
            CameraComponent camera = cameraNode.camera;
            BattleCameraComponent battleCamera = cameraNode.battleCamera;
            ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect = weapon.shaftAimingCameraConfigEffect;
            float recoveringFovSpeed = shaftAimingCameraConfigEffect.RecoveringFovSpeed;
            float optimalFOV = battleCamera.OptimalFOV;
            camera.FOV += recoveringFovSpeed * evt.DeltaTime;

            if (camera.FOV >= optimalFOV) {
                camera.FOV = optimalFOV;
                Entity entity = cameraNode.Entity;
                entity.RemoveComponent<ShaftAimingCameraFOVRecoveringComponent>();
                entity.AddComponent<CameraFOVUpdateComponent>();
            }
        }

        public class ShaftAimingCameraEffectConfigNode : Node {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;

            public ShaftStateControllerComponent shaftStateController;
        }

        public class ShaftWeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public ShaftStateControllerComponent shaftStateController;

            public WeaponInstanceComponent weaponInstance;
        }

        public class ShaftWeaponKickbackNode : ShaftWeaponNode {
            public ShaftAimingCameraKickbackComponent shaftAimingCameraKickback;
        }

        public class AimingWorkActivationStateNode : ShaftWeaponNode {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;

            public ShaftAimingRotationConfigComponent shaftAimingRotationConfig;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftStateConfigComponent shaftStateConfig;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class AimingIdleStateNode : ShaftWeaponNode {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class AimingIdleKickbackStateNode : AimingIdleStateNode {
            public ShaftAimingCameraKickbackComponent shaftAimingCameraKickback;
        }

        public class AimingWorkingStateNode : ShaftWeaponNode {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;

            public ShaftAimingRotationConfigComponent shaftAimingRotationConfig;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class InitialCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;

            public CameraRootTransformComponent cameraRootTransform;
        }

        [Not(typeof(CameraFOVUpdateComponent))]
        public class CameraRecoveringNode : InitialCameraNode {
            public ShaftAimingCameraFOVRecoveringComponent shaftAimingCameraFOVRecovering;
        }

        [Not(typeof(ApplyCameraTransformComponent))]
        public class AimingCameraNode : InitialCameraNode {
            public CameraTransformDataComponent cameraTransformData;

            public ShaftAimingCameraComponent shaftAimingCamera;
        }
    }
}