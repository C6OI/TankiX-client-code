using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ApplyCameraTransformSystem : ECSSystem {
        [OnEventFire]
        public void InitTimeSmoothing(NodeAddedEvent evt, BattleCameraNode battleCamera) {
            CameraComponent camera = battleCamera.camera;
            TransformTimeSmoothingComponent transformTimeSmoothingComponent = new();
            transformTimeSmoothingComponent.Transform = camera.Camera.transform;
            transformTimeSmoothingComponent.UseCorrectionByFrameLeader = true;
            battleCamera.Entity.AddComponent(transformTimeSmoothingComponent);
        }

        [OnEventFire]
        public void ResetTimeSmoothing(NodeRemoveEvent evt, BattleCameraNode battleCamera) =>
            battleCamera.Entity.RemoveComponent<TransformTimeSmoothingComponent>();

        [OnEventFire]
        public void ApplyCameraTransform(ApplyCameraTransformEvent e, BattleCameraNode battleCamera) {
            CameraComponent camera = battleCamera.camera;
            CameraTransformDataComponent cameraTransformData = battleCamera.cameraTransformData;
            Transform transform = camera.Camera.transform;
            Vector3 position = cameraTransformData.Data.Position;
            Quaternion rotation = cameraTransformData.Data.Rotation;
            float t = 1f;
            float t2 = 1f;
            float? deltaTime = e.deltaTime;

            if (deltaTime.HasValue) {
                float value = e.deltaTime.Value;
                float? positionSmoothingRatio = e.positionSmoothingRatio;

                t = !positionSmoothingRatio.HasValue ? battleCamera.applyCameraTransform.positionSmoothingRatio
                        : positionSmoothingRatio.Value;

                float? rotationSmoothingRatio = e.rotationSmoothingRatio;

                t2 = !rotationSmoothingRatio.HasValue ? battleCamera.applyCameraTransform.rotationSmoothingRatio
                         : rotationSmoothingRatio.Value;

                battleCamera.applyCameraTransform.positionSmoothingRatio = t;
                battleCamera.applyCameraTransform.rotationSmoothingRatio = t2;
                t *= value;
                t2 *= value;
            }

            transform.position = Vector3.Lerp(transform.position, position, t);
            Vector3 eulerAngles = rotation.eulerAngles;
            Vector3 eulerAngles2 = transform.rotation.eulerAngles;
            float x = Mathf.LerpAngle(eulerAngles2.x, eulerAngles.x, t2);
            float y = Mathf.LerpAngle(eulerAngles2.y, eulerAngles.y, t2);
            transform.rotation = Quaternion.Euler(new Vector3(x, y, 0f));
            ScheduleEvent<TransformTimeSmoothingEvent>(battleCamera);
        }

        public class BattleCameraNode : NotDeletedEntityNode {
            public ApplyCameraTransformComponent applyCameraTransform;

            public BattleCameraComponent battleCamera;
            public CameraComponent camera;

            public CameraTransformDataComponent cameraTransformData;
        }
    }
}