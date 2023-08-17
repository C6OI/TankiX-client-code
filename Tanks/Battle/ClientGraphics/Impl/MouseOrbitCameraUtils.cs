using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class MouseOrbitCameraUtils {
        public static Vector3 NormalizeEuler(Vector3 euler) => NormalizeEuler(euler,
            MouseOrbitCameraConstants.X_MIN_ANGLE,
            MouseOrbitCameraConstants.X_MAX_ANGLE);

        static Vector3 NormalizeEuler(Vector3 euler, float xMinAngle, float xMaxAngle) {
            euler.x = Mathf.Clamp((euler.x + 180f) % 360f - 180f, xMinAngle, xMaxAngle);
            euler.y = (euler.y + 360f) % 360f;
            euler.z = 0f;
            return euler;
        }

        public static Vector3 GetClippedPosition(Vector3 centralPosition, Vector3 targetPosition, float maxDistance) {
            RaycastHit hitInfo;

            if (Physics.Linecast(centralPosition, targetPosition, out hitInfo, LayerMasks.STATIC)) {
                targetPosition = hitInfo.point;
            }

            targetPosition -= (targetPosition - centralPosition).normalized * MouseOrbitCameraConstants.CLIP_DISTANCE;

            if (Physics.Linecast(targetPosition,
                    targetPosition + Vector3.down * MouseOrbitCameraConstants.CLIP_DISTANCE,
                    out hitInfo,
                    LayerMasks.STATIC)) {
                targetPosition.y = hitInfo.point.y + MouseOrbitCameraConstants.CLIP_DISTANCE;
            }

            float num = 1f -
                        new Vector2(targetPosition.x - centralPosition.x, targetPosition.z - centralPosition.z).magnitude /
                        maxDistance;

            targetPosition.y += num * MouseOrbitCameraConstants.PROXIMITY_ELEVATION;
            return targetPosition;
        }

        public static TransformData GetTargetMouseOrbitCameraTransformData(Transform cameraTargetTransform,
            float mouseOrbitCameraDistance, Quaternion mouseOrbitCameraTargetRotation) {
            TransformData result = default;
            result.Rotation = Quaternion.Euler(NormalizeEuler(mouseOrbitCameraTargetRotation.eulerAngles));
            Vector3 position = cameraTargetTransform.position;
            Vector3 targetPosition = position + result.Rotation * new Vector3(0f, 0f, 0f - mouseOrbitCameraDistance);
            targetPosition = GetClippedPosition(position, targetPosition, MouseOrbitCameraConstants.MAX_DISTANCE);
            result.Position = targetPosition;
            return result;
        }
    }
}