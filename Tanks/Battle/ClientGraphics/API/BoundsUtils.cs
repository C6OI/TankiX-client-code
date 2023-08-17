using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class BoundsUtils {
        public static Bounds TransformBounds(Bounds bounds, Matrix4x4 matrix) {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Bounds result = new(matrix.MultiplyPoint3x4(min), Vector3.zero);
            result.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(min.x, min.y, max.z)));
            result.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(min.x, max.y, min.z)));
            result.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(min.x, max.y, max.z)));
            result.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(max.x, min.y, min.z)));
            result.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(max.x, min.y, max.z)));
            result.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(max.x, max.y, min.z)));
            result.Encapsulate(matrix.MultiplyPoint3x4(max));
            return result;
        }

        public static void DebugBounds(Bounds bounds, Color color) => DebugBounds(bounds, color, Matrix4x4.identity);

        public static void DebugBounds(Bounds bounds, Color color, Matrix4x4 boundsSpaceToWorld) =>
            DebugBounds(bounds.min, bounds.max, color, boundsSpaceToWorld);

        public static void DebugBounds(Vector3 min, Vector3 max, Color color, Matrix4x4 boundsSpaceToWorld) {
            Vector3 v = new(min.x, min.y, min.z);
            Vector3 v2 = new(max.x, min.y, min.z);
            Vector3 v3 = new(max.x, max.y, min.z);
            Vector3 v4 = new(min.x, max.y, min.z);
            Vector3 v5 = new(min.x, min.y, max.z);
            Vector3 v6 = new(max.x, min.y, max.z);
            Vector3 v7 = new(max.x, max.y, max.z);
            Vector3 v8 = new(min.x, max.y, max.z);
            v = boundsSpaceToWorld.MultiplyPoint3x4(v);
            v2 = boundsSpaceToWorld.MultiplyPoint3x4(v2);
            v3 = boundsSpaceToWorld.MultiplyPoint3x4(v3);
            v4 = boundsSpaceToWorld.MultiplyPoint3x4(v4);
            v5 = boundsSpaceToWorld.MultiplyPoint3x4(v5);
            v6 = boundsSpaceToWorld.MultiplyPoint3x4(v6);
            v7 = boundsSpaceToWorld.MultiplyPoint3x4(v7);
            v8 = boundsSpaceToWorld.MultiplyPoint3x4(v8);
            Debug.DrawLine(v, v2, color, 0f);
            Debug.DrawLine(v, v2, color, 0f);
            Debug.DrawLine(v2, v3, color, 0f);
            Debug.DrawLine(v3, v4, color, 0f);
            Debug.DrawLine(v4, v, color, 0f);
            Debug.DrawLine(v5, v6, color, 0f);
            Debug.DrawLine(v6, v7, color, 0f);
            Debug.DrawLine(v7, v8, color, 0f);
            Debug.DrawLine(v8, v5, color, 0f);
            Debug.DrawLine(v, v5, color, 0f);
            Debug.DrawLine(v2, v6, color, 0f);
            Debug.DrawLine(v4, v8, color, 0f);
            Debug.DrawLine(v3, v7, color, 0f);
        }

        public static void DrawBoundsGizmo(Bounds bounds, Color color, Matrix4x4 boundsSpaceToWorld) =>
            DrawBoundsGizmo(bounds.min, bounds.max, color, boundsSpaceToWorld);

        public static void DrawBoundsGizmo(Vector3 min, Vector3 max, Color color, Matrix4x4 boundsSpaceToWorld) {
            Vector3 v = new(min.x, min.y, min.z);
            Vector3 v2 = new(max.x, min.y, min.z);
            Vector3 v3 = new(max.x, max.y, min.z);
            Vector3 v4 = new(min.x, max.y, min.z);
            Vector3 v5 = new(min.x, min.y, max.z);
            Vector3 v6 = new(max.x, min.y, max.z);
            Vector3 v7 = new(max.x, max.y, max.z);
            Vector3 v8 = new(min.x, max.y, max.z);
            v = boundsSpaceToWorld.MultiplyPoint3x4(v);
            v2 = boundsSpaceToWorld.MultiplyPoint3x4(v2);
            v3 = boundsSpaceToWorld.MultiplyPoint3x4(v3);
            v4 = boundsSpaceToWorld.MultiplyPoint3x4(v4);
            v5 = boundsSpaceToWorld.MultiplyPoint3x4(v5);
            v6 = boundsSpaceToWorld.MultiplyPoint3x4(v6);
            v7 = boundsSpaceToWorld.MultiplyPoint3x4(v7);
            v8 = boundsSpaceToWorld.MultiplyPoint3x4(v8);
            Gizmos.color = color;
            Gizmos.DrawLine(v, v2);
            Gizmos.DrawLine(v2, v3);
            Gizmos.DrawLine(v3, v4);
            Gizmos.DrawLine(v4, v);
            Gizmos.DrawLine(v5, v6);
            Gizmos.DrawLine(v6, v7);
            Gizmos.DrawLine(v7, v8);
            Gizmos.DrawLine(v8, v5);
            Gizmos.DrawLine(v, v5);
            Gizmos.DrawLine(v2, v6);
            Gizmos.DrawLine(v4, v8);
            Gizmos.DrawLine(v3, v7);
        }
    }
}