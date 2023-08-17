using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GoCameraSystem : ECSSystem {
        [OnEventFire]
        public void InitGoCamera(NodeAddedEvent evt, GoCameraNode goCameraNode,
            [JoinAll] CameraTargetNode cameraTargetNode) {
            int goCameraIndex = Random.Range(0, goCameraNode.goCameraPointsUnity.goCameraPoints.Length);
            goCameraNode.goCamera.goCameraIndex = goCameraIndex;
        }

        [OnEventComplete]
        public void UpdateGoCamera(TimeUpdateEvent evt, GoCameraNode goCameraNode,
            [JoinAll] CameraTargetNode cameraTargetNode) {
            CameraTransformDataComponent cameraTransformData = goCameraNode.cameraTransformData;
            GoCameraComponent goCamera = goCameraNode.goCamera;
            CameraTargetComponent cameraTarget = cameraTargetNode.cameraTarget;
            Transform transform = cameraTarget.TargetObject.transform;
            GoCameraPointsUnityComponent goCameraPointsUnity = goCameraNode.goCameraPointsUnity;
            int goCameraIndex = goCamera.goCameraIndex;
            GoCameraPoint goCameraPoint = goCameraPointsUnity.goCameraPoints[goCameraIndex];
            Vector3 vector = transform.rotation * goCameraPoint.poistion;

            cameraTransformData.Data = new TransformData {
                Position = transform.position + vector,
                Rotation = Quaternion.Euler(transform.rotation.eulerAngles + goCameraPoint.rotation)
            };

            ScheduleEvent<ApplyCameraTransformEvent>(goCameraNode);
        }

        public class GoCameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraTransformDataComponent cameraTransformData;

            public GoCameraComponent goCamera;
            public GoCameraPointsUnityComponent goCameraPointsUnity;
        }

        public class CameraTargetNode : Node {
            public CameraTargetComponent cameraTarget;
        }
    }
}