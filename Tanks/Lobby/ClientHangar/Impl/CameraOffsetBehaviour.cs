using System;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class CameraOffsetBehaviour : MonoBehaviour {
        [SerializeField] float offsetX;

        [SerializeField] float offsetY;

        Camera camera;

        void Awake() => camera = GetComponent<Camera>();

        void LateUpdate() {
            float num = 1f / Mathf.Tan(0.5f * camera.fieldOfView * ((float)Math.PI / 180f));
            float num2 = camera.nearClipPlane / num;
            float num3 = camera.aspect * num2;
            float num4 = -1f - offsetX;
            float num5 = 1f - offsetX;
            float num6 = 1f + offsetY;
            float num7 = -1f + offsetY;

            Matrix4x4 projectionMatrix = PerspectiveOffCenter(num3 * num4,
                num3 * num5,
                num2 * num7,
                num2 * num6,
                camera.nearClipPlane,
                camera.farClipPlane);

            camera.projectionMatrix = projectionMatrix;
        }

        void OnEnable() => Cursor.visible = true;

        void OnDisable() {
            Camera component = GetComponent<Camera>();
            component.ResetProjectionMatrix();
        }

        static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far) {
            float value = 2f * near / (right - left);
            float value2 = 2f * near / (top - bottom);
            float value3 = (right + left) / (right - left);
            float value4 = (top + bottom) / (top - bottom);
            float value5 = (0f - (far + near)) / (far - near);
            float value6 = (0f - 2f * far * near) / (far - near);
            float value7 = -1f;
            Matrix4x4 result = default;
            result[0, 0] = value;
            result[0, 1] = 0f;
            result[0, 2] = value3;
            result[0, 3] = 0f;
            result[1, 0] = 0f;
            result[1, 1] = value2;
            result[1, 2] = value4;
            result[1, 3] = 0f;
            result[2, 0] = 0f;
            result[2, 1] = 0f;
            result[2, 2] = value5;
            result[2, 3] = value6;
            result[3, 0] = 0f;
            result[3, 1] = 0f;
            result[3, 2] = value7;
            result[3, 3] = 0f;
            return result;
        }
    }
}