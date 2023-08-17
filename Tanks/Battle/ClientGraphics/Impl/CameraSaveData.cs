using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraSaveData {
        CameraSaveData() { }

        public string UserUid { get; private set; }

        public CameraType Type { get; private set; }

        public TransformData TransformData { get; private set; }

        public float FollowCameraBezierPositionRatio { get; private set; }

        public float FollowCameraBezierPositionRatioOffset { get; private set; }

        public float MouseOrbitDistance { get; private set; }

        public Quaternion MouseOrbitTargetRotation { get; private set; }

        public static CameraSaveData CreateFreeData(Transform transform) {
            CameraSaveData cameraSaveData = new();
            cameraSaveData.Type = CameraType.Free;

            cameraSaveData.TransformData = new TransformData {
                Position = transform.position,
                Rotation = transform.rotation
            };

            return cameraSaveData;
        }

        public static CameraSaveData CreateMouseOrbitData(string userUid, float mouseOrbitDistance,
            Quaternion mouseOrbitTargetRotation) {
            CameraSaveData cameraSaveData = new();
            cameraSaveData.UserUid = userUid;
            cameraSaveData.Type = CameraType.MouseOrbit;
            cameraSaveData.MouseOrbitDistance = mouseOrbitDistance;
            cameraSaveData.MouseOrbitTargetRotation = mouseOrbitTargetRotation;
            return cameraSaveData;
        }

        public static CameraSaveData CreateFollowData(string userUid, float followCameraBezierPositionRatio,
            float followCameraBezierPositionRatioOffset) {
            CameraSaveData cameraSaveData = new();
            cameraSaveData.UserUid = userUid;
            cameraSaveData.Type = CameraType.Follow;
            cameraSaveData.FollowCameraBezierPositionRatio = followCameraBezierPositionRatio;
            cameraSaveData.FollowCameraBezierPositionRatioOffset = followCameraBezierPositionRatioOffset;
            return cameraSaveData;
        }
    }
}