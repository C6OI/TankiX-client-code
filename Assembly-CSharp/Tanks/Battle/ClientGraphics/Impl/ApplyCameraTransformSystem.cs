using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ApplyCameraTransformSystem : ECSSystem {
        [OnEventFire]
        public void InitTimeSmoothing(NodeAddedEvent evt, BattleCameraNode battleCamera) {
            CameraComponent camera = battleCamera.camera;
            TransformTimeSmoothingComponent transformTimeSmoothingComponent = new();
            transformTimeSmoothingComponent.Transform = battleCamera.cameraRootTransform.Root;
            transformTimeSmoothingComponent.UseCorrectionByFrameLeader = true;
            battleCamera.Entity.AddComponent(transformTimeSmoothingComponent);
        }

        [OnEventFire]
        public void ResetTimeSmoothing(NodeRemoveEvent evt, BattleCameraNode battleCamera) {
            battleCamera.Entity.RemoveComponent<TransformTimeSmoothingComponent>();
        }

        [OnEventFire]
        public void ApplyCameraTransform(ApplyCameraTransformEvent e, BattleCameraNode battleCamera) {
            CameraComponent camera = battleCamera.camera;
            CameraTransformDataComponent cameraTransformData = battleCamera.cameraTransformData;
            Transform root = battleCamera.cameraRootTransform.Root;
            Vector3 position = cameraTransformData.Data.Position;
            Quaternion rotation = cameraTransformData.Data.Rotation;
            float t = 1f;
            float t2 = 1f;

            if (e.DeltaTimeValid) {
                float deltaTime = e.DeltaTime;
                t = !e.PositionSmoothingRatioValid ? battleCamera.applyCameraTransform.positionSmoothingRatio : e.PositionSmoothingRatio;
                t2 = !e.RotationSmoothingRatioValid ? battleCamera.applyCameraTransform.rotationSmoothingRatio : e.RotationSmoothingRatio;
                battleCamera.applyCameraTransform.positionSmoothingRatio = t;
                battleCamera.applyCameraTransform.rotationSmoothingRatio = t2;
                t *= deltaTime;
                t2 *= deltaTime;
            }

            root.SetPositionSafe(Vector3.Lerp(root.position, position, t));
            Vector3 eulerAngles = rotation.eulerAngles;
            Vector3 eulerAngles2 = root.rotation.eulerAngles;
            float x = Mathf.LerpAngle(eulerAngles2.x, eulerAngles.x, t2);
            float y = Mathf.LerpAngle(eulerAngles2.y, eulerAngles.y, t2);
            root.rotation = Quaternion.Euler(new Vector3(x, y, 0f));
            ScheduleEvent<TransformTimeSmoothingEvent>(battleCamera);
        }

        public class BattleCameraNode : NotDeletedEntityNode {
            public ApplyCameraTransformComponent applyCameraTransform;

            public BattleCameraComponent battleCamera;
            public CameraComponent camera;

            public CameraRootTransformComponent cameraRootTransform;

            public CameraTransformDataComponent cameraTransformData;
        }
    }
}