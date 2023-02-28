using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public static class MathUtil {
        const float TWO_PI = (float)Math.PI * 2f;

        public static float Sign(float value) {
            if (value < 0f) {
                return -1f;
            }

            if (value > 0f) {
                return 1f;
            }

            return 0f;
        }

        public static float SignEpsilon(float value, float eps) {
            if (value < 0f - eps) {
                return -1f;
            }

            if (value > eps) {
                return 1f;
            }

            return 0f;
        }

        public static int GetBitValue(int value, int bitIndex) => value >> bitIndex & 1;

        public static Matrix4x4 FromAxisAngle(Vector3 axis, float angle) {
            float num = Mathf.Cos(angle);
            float num2 = Mathf.Sin(angle);
            float num3 = 1f - num;
            float x = axis.x;
            float y = axis.y;
            float z = axis.z;
            Matrix4x4 result = default;
            result.m00 = num3 * x * x + num;
            result.m01 = num3 * x * y - z * num2;
            result.m02 = num3 * x * z + y * num2;
            result.m10 = num3 * x * y + z * num2;
            result.m11 = num3 * y * y + num;
            result.m12 = num3 * y * z - x * num2;
            result.m20 = num3 * x * z - y * num2;
            result.m21 = num3 * y * z + x * num2;
            result.m22 = num3 * z * z + num;
            return result;
        }

        public static Matrix4x4 SetRotationMatrix(Vector3 eulerAngles) {
            float num = Mathf.Cos(eulerAngles.x);
            float num2 = Mathf.Sin(eulerAngles.x);
            float num3 = Mathf.Cos(eulerAngles.y);
            float num4 = Mathf.Sin(eulerAngles.y);
            float num5 = Mathf.Cos(eulerAngles.z);
            float num6 = Mathf.Sin(eulerAngles.z);
            float num7 = num5 * num4;
            float num8 = num6 * num4;
            Matrix4x4 result = default;
            result.m00 = num5 * num3;
            result.m01 = num7 * num2 - num6 * num;
            result.m02 = num7 * num + num6 * num2;
            result.m10 = num6 * num3;
            result.m11 = num8 * num2 + num5 * num;
            result.m12 = num8 * num - num5 * num2;
            result.m20 = 0f - num4;
            result.m21 = num3 * num2;
            result.m22 = num3 * num;
            return result;
        }

        public static Vector3 GetEulerAngles(Matrix4x4 m) {
            Vector3 vector = !(-1f < m.m20) || !(m.m20 < 1f) ? new Vector3(0f, 0.5f * (!(m.m20 <= -1f) ? -(float)Math.PI : (float)Math.PI), Mathf.Atan2(0f - m.m01, m.m11))
                                 : new Vector3(Mathf.Atan2(m.m21, m.m22), 0f - Mathf.Asin(m.m20), Mathf.Atan2(m.m10, m.m00));

            return vector * 57.29578f;
        }

        public static float Snap(float value, float snapValue, float epsilon) {
            if (value > snapValue - epsilon && value < snapValue + epsilon) {
                return snapValue;
            }

            return value;
        }

        public static float ClampAngle180(float degrees) => (degrees % 360f + 540f) % 360f - 180f;

        public static float ClampAngle(float radians) {
            radians %= (float)Math.PI * 2f;
            return ClampAngleFast(radians);
        }

        public static float ClampAngleFast(float radians) {
            if (radians < -(float)Math.PI) {
                return (float)Math.PI * 2f + radians;
            }

            if (radians > (float)Math.PI) {
                return radians - (float)Math.PI * 2f;
            }

            return radians;
        }

        public static Quaternion AddScaledVector(Quaternion q, Vector3 v, float scale) {
            float num = v.x * scale;
            float num2 = v.y * scale;
            float num3 = v.z * scale;
            float num4 = (0f - q.x) * num - q.y * num2 - q.z * num3;
            float num5 = num * q.w + num2 * q.z - num3 * q.y;
            float num6 = num2 * q.w + num3 * q.x - num * q.z;
            float num7 = num3 * q.w + num * q.y - num2 * q.x;
            Quaternion result = new(q.x + 0.5f * num5, q.y + 0.5f * num6, q.z + 0.5f * num7, q.w + 0.5f * num4);
            float num8 = result.w * result.w + result.x * result.x + result.y * result.y + result.z * result.z;

            if (num8 == 0f) {
                result.w = 1f;
            } else {
                num8 = 1f / Mathf.Sqrt(num8);
                result.w *= num8;
                result.x *= num8;
                result.y *= num8;
                result.z *= num8;
            }

            return result;
        }

        public static bool NearlyEqual(float a, float b, float epsilon) => Mathf.Abs(a - b) < epsilon;

        public static bool NearlyEqual(Vector3 a, Vector3 b, float epsilon) => NearlyEqual(a.x, b.x, epsilon) && NearlyEqual(a.y, b.y, epsilon) && NearlyEqual(a.z, b.z, epsilon);

        public static Vector3 WorldPositionToLocalPosition(Vector3 position, GameObject gameObject) => gameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(position);

        public static Vector3 LocalPositionToWorldPosition(Vector3 position, GameObject gameObject) => gameObject.transform.localToWorldMatrix.MultiplyPoint3x4(position);
    }
}