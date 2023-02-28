using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class Bezier {
        public static float PointOnCurve(float t, params float[] points) {
            float num = 1f - t;
            int num2 = points.Length;

            while (num2-- > 1) {
                for (int i = 0; i < num2; i++) {
                    points[i] = points[i] * num + points[i + 1] * t;
                }
            }

            return points[0];
        }

        public static Vector2 PointOnCurve(float t, params Vector2[] points) {
            float num = 1f - t;
            int num2 = points.Length;

            while (num2-- > 1) {
                for (int i = 0; i < num2; i++) {
                    points[i] = points[i] * num + points[i + 1] * t;
                }
            }

            return points[0];
        }

        public static Vector3 PointOnCurve(float t, params Vector3[] points) {
            float num = 1f - t;
            int num2 = points.Length;

            while (num2-- > 1) {
                for (int i = 0; i < num2; i++) {
                    points[i] = points[i] * num + points[i + 1] * t;
                }
            }

            return points[0];
        }

        public static Vector3 PointOnCurve(float t, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4) {
            float num = 1f - t;
            return (point1 * num + point2 * (3f * t)) * (num * num) + (point3 * (3f * num) + point4 * t) * (t * t);
        }

        public static Vector2 PointOnCurve(float t, Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4) {
            float num = 1f - t;
            return (point1 * num + point2 * (3f * t)) * (num * num) + (point3 * (3f * num) + point4 * t) * (t * t);
        }

        public static float PointOnCurve(float t, float point1, float point2, float point3, float point4) {
            float num = 1f - t;
            return (point1 * num + point2 * (3f * t)) * (num * num) + (point3 * (3f * num) + point4 * t) * (t * t);
        }

        public static Vector3 PointOnCurve(float t, Vector3 point1, Vector3 point2, Vector3 point3) {
            float num = 1f - t;
            return (point1 * num + point2 * (2f * t)) * num + point3 * (t * t);
        }

        public static Vector2 PointOnCurve(float t, Vector2 point1, Vector2 point2, Vector2 point3) {
            float num = 1f - t;
            return (point1 * num + point2 * (2f * t)) * num + point3 * (t * t);
        }

        public static float PointOnCurve(float t, float point1, float point2, float point3) {
            float num = 1f - t;
            return (point1 * num + point2 * (2f * t)) * num + point3 * (t * t);
        }
    }
}